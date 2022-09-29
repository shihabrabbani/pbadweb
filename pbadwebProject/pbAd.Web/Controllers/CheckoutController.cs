#region Usings
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Service.ABPrintClassifiedTexts;
using pbAd.Service.Agencies;
using pbAd.Service.CacheManagerServices;
using pbAd.Service.DefaultDiscounts;
using pbAd.Service.EditionDistricts;
using pbAd.Service.EditionPages;
using pbAd.Service.Editions;
using pbAd.Service.OfferEditions;
using pbAd.Service.PaymentGateways;
using pbAd.Service.PaymentGateways.Models;
using pbAd.Service.RatePrintClassifiedTexts;
using pbAd.Service.SSLPaymentTransactions;
using pbAd.Web.Infrastructure.Helpers;
using pbAd.Web.ViewModels.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Web.Controllers
{
    public class CheckoutController : AdminBaseController
    {
        #region Private Methods
        private readonly IEditionService editionService;
        private readonly IEditionPageService editionPageService;
        private readonly IABPrintClassifiedTextService aBPrintClassifiedTextService;
        private readonly IRatePrintClassifiedTextService ratePrintClassifiedTextService;
        private readonly IOfferEditionService offerEditionService;
        private readonly IDefaultDiscountService defaultDiscountService;
        private readonly IEditionDistrictService editionDistrictService;
        private readonly ISSLPaymentTransactionService sslPaymentTransactionService;
      
        private readonly IConfiguration configuration;
        private readonly IAgencyService agencyService;
        private readonly ICacheManagerService cacheManagerService;
        #endregion

        #region Ctor

        public CheckoutController(IEditionService editionService,
              IABPrintClassifiedTextService aBPrintClassifiedTextService,
            IRatePrintClassifiedTextService ratePrintClassifiedTextService,
            IOfferEditionService offerEditionService,
             IEditionPageService editionPageService,
            IDefaultDiscountService defaultDiscountService,
            IAgencyService agencyService,
           
            IConfiguration configuration,
            ICacheManagerService cacheManagerService,
            ISSLPaymentTransactionService sslPaymentTransactionService,
            IEditionDistrictService editionDistrictService
            )
        {
            this.editionService = editionService;
            this.editionPageService = editionPageService;
            this.aBPrintClassifiedTextService = aBPrintClassifiedTextService;
            this.ratePrintClassifiedTextService = ratePrintClassifiedTextService;
            this.offerEditionService = offerEditionService;
            this.defaultDiscountService = defaultDiscountService;
            this.editionDistrictService = editionDistrictService;
            this.agencyService = agencyService;

            this.cacheManagerService = cacheManagerService;
            
            this.configuration = configuration;
            this.sslPaymentTransactionService = sslPaymentTransactionService;
        }

        #endregion

        #region Payment

        [Route("/checkout/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(int id, string bookingno)
        {
            var bookingStep = BookingStepConstants.Booked;
            var classifiedText = await aBPrintClassifiedTextService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

            var classifiedTextview = await aBPrintClassifiedTextService.GetDetailsView(id, bookingno, bookingStep);
            if (classifiedText == null)
                return Redirect("/adbook/booknow");

            var classifiedTextDetailListing = await aBPrintClassifiedTextService.GetABPrintClassifiedTextDetailListing(id);

            if (!classifiedTextDetailListing.Any())
                return Redirect("/adbook/booknow");

            var edtions = await editionService.GetAllEditions();

            var basedOfferDates = classifiedTextDetailListing.OrderBy(o => o.PublishDate).Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();

            var basedOfferDatesInString = String.Join(", ", basedOfferDates);

            var defaultEdtion = edtions.FirstOrDefault(f => f.EditionId == CurrentLoginUser.EditionId);

            var filteredEditions = edtions.Where(f => f.EditionId != EditionConstants.National).ToList();

            var adContent = "";

            var abPrintClassifiedTextDetail = classifiedTextDetailListing.FirstOrDefault();
            if (abPrintClassifiedTextDetail != null)
                adContent = abPrintClassifiedTextDetail.AdContent;

            var editionDistrictList = await editionDistrictService.GetAllEditionDistricts();
            var editionDistrict = editionDistrictList.FirstOrDefault(f => f.EditionId == defaultEdtion.EditionId);
            var defaultEditionToolTip = "";
            if (editionDistrict != null)
                defaultEditionToolTip = editionDistrict.HoverTextValue;

            //rate
            var editionId = CurrentLoginUser.EditionId ?? 0;
            var rateClassifiedText = await ratePrintClassifiedTextService.GetDefaultRatePrintClassifiedText(editionId);

            var model = new CheckoutViewModel
            {
                ABPrintClassifiedText = classifiedText,
                ABPrintClassifiedTextView = classifiedTextview,
                Editions = filteredEditions,
                BasedOfferDatesInString = basedOfferDatesInString,
                ABPrintClassifiedTextId = id,
                BookingNo = bookingno,
                DefaultEditionName = defaultEdtion.EditionName,
                DefaultEditionId = defaultEdtion.EditionId,
                DefaultEditionToolTip = defaultEditionToolTip,
                EditionDistrictList = editionDistrictList,
                ManualDiscountPercentage = 0,
                Rate = rateClassifiedText.PerWordRate,
                BillDate = basedOfferDates.FirstOrDefault()
            };

            //Get default payment mode & discount
            await GetDefaultPaymentModeAndDiscount(model);

            classifiedText.AdContent = adContent;

            //get ad enhancement and ad content
            model.AdEnhancement = GetAdEnhancementAndAdContent(classifiedText);
            model.AdContent = classifiedText.AdContent;

            //store classified text into session
            HttpContext.Session.SetObject(SessionKeyConstants.ABPrintClassifiedText, classifiedText);
            HttpContext.Session.SetInt(SessionKeyConstants.ClassifiedTextTotalPublishDates, basedOfferDates.Count());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                if (!model.AcceptTermsAndConditions)
                    return Json(new { status = false, message = "Please accept terms and conditions", type = ActionResultTypeConstants.Message });

                //get stored classified text from session
                var classifiedText = HttpContext.Session.GetObject<ABPrintClassifiedText>(SessionKeyConstants.ABPrintClassifiedText);

                if (classifiedText == null)
                    return Json(new { status = false, message = "Warning! Classified Text Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                if ((classifiedText.ABPrintClassifiedTextId != model.ABPrintClassifiedTextId)
                    || (classifiedText.BookingNo != model.BookingNo))
                    return Json(new { status = false, message = "Warning! Classified Display Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                if (model.NationalEdition)
                {
                    var defaultEditionId = EditionConstants.National;
                    model.EditionIds = new List<int>();
                    model.EditionIds.Add(defaultEditionId);
                }

                if (model.EditionIds == null || !model.EditionIds.Any())
                    return Json(new { status = false, message = "Warning! Please check atlest one edition.", type = ActionResultTypeConstants.Message });

                //re-generate ab print classified text
                var updateClassifiedText = await RegenerateABPrintClassifiedTextInfo(model);

                //Populate ab print classified text detail
                var abPrintClassifiedTextDetailListing = await PopulateABPrintClassifiedTextDetail(model);

                var checkProcessRequest = new CheckoutProcessModel
                {
                    ABPrintClassifiedTextId = model.ABPrintClassifiedTextId,
                    ABPrintClassifiedText = updateClassifiedText,
                    ABPrintClassifiedTextDetailListing = abPrintClassifiedTextDetailListing
                };

                //let's Add ABPrintClassifiedText Detail
                var response = await aBPrintClassifiedTextService.CheckoutClassifiedTextProcess(checkProcessRequest);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/{model.ABPrintClassifiedTextId}/Confirmation/{model.BookingNo}";

                if (CurrentLoginUser.IsCorrespondentUser)
                    returnUrl = $"/checkout/{model.ABPrintClassifiedTextId}/payment-complete/{model.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [AllowAnonymous]
        [Route("/checkout/{id}/confirmation/{bookingno}")]
        [Route("/checkout/{id}/confirmation/{bookingno}/mode/{paymentmode}")]
        [Route("/checkout/{id}/confirmation/{bookingno}/mode/{paymentmode}/adtype/{adtype}")]
        public async Task<IActionResult> Confirmation(int id, string bookingno, int paymentmode, string adtype)
        {
            try
            {
                var updatedBy = CurrentLoginUser.UserId;
                if (paymentmode == CheckoutPaymentTypeConstants.Card)
                {
                    var userInfo = cacheManagerService.GetSSLInUserInfo(bookingno);
                    ViewBag.SSLUserInfo = userInfo;

                    updatedBy = userInfo.UserId;

                    //Populate SSL Payment Transaction
                    SSLPaymentTransaction updateSSLPaymentTransaction = PopulateSSLPaymentTransaction(id, adtype);

                    var updateClassifiedText = new ABPrintClassifiedText
                    {
                        ABPrintClassifiedTextId = id,
                        ApprovalStatus = ApproveStatusConstants.Approved,
                        PaymentStatus = PaymentStatusConstants.Paid,
                        AdStatus = AdStatusConstants.Confirmed_And_Paid,
                        ActualReceived = updateSSLPaymentTransaction.Amount,
                        PaymentTrxId = updateSSLPaymentTransaction.Bank_Tran_Id,
                        PaymentModeId = paymentmode,
                        MoneyReceiptNo= updateSSLPaymentTransaction.Bank_Tran_Id,
                        PaymentMobileNumber= PaymentModeConstants.GetText(PaymentModeConstants.SSL),
                        ModifiedBy = updatedBy
                    };
                    //let's update payment mode
                    await aBPrintClassifiedTextService.UpdatePaymentMode(updateClassifiedText);

                    await sslPaymentTransactionService.Update(updateSSLPaymentTransaction);
                }

                ViewBag.BookingNo = bookingno;
                //Clear Session Data
                ClearSessionData();
                return View();
            }
            catch
            {
                return Redirect("/");
            }
        }

        #endregion

        #region Complete Payment

        [Route("/checkout/{id}/payment-complete/{bookingno}")]
        public async Task<IActionResult> PaymentComplete(int id, string bookingno)
        {
            if (!CurrentLoginUser.IsCorrespondentUser)
                return Redirect("/adbook/booknow");

            var bookingStep = BookingStepConstants.Checkout;
            var classifiedText = await aBPrintClassifiedTextService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

            if (classifiedText == null)
                return Redirect("/adbook/booknow");

            var netPayable = Convert.ToInt32(Math.Round(classifiedText.NetAmount) + Math.Round(classifiedText.VATAmount) - Math.Round(classifiedText.Commission));

            var model = new PaymentViewModel
            {
                MasterId = classifiedText.ABPrintClassifiedTextId,
                BookingNo = bookingno,
                NetPayable = netPayable
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/{id}/payment-complete/{bookingno}")]
        public async Task<IActionResult> PaymentComplete(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                if (model.PaymentType == CheckoutPaymentTypeConstants.Direct)
                {
                    if (model.PaymentMethod <= 0)
                        return Json(new { status = false, message = "Warning! Please Select any one of the payment method like bKash, Rocket or Nogod.", type = ActionResultTypeConstants.Message });

                    if (string.IsNullOrWhiteSpace(model.PaymentMobileNumber))
                        return Json(new { status = false, message = "Warning! Mobile Number must be 11 digits", type = ActionResultTypeConstants.Message });

                    if (model.PaymentMobileNumber.Length != 11)
                        return Json(new { status = false, message = "Warning! Mobile Number must be 11 digits", type = ActionResultTypeConstants.Message });

                    if (model.PaymentPaidAmount <= 0)
                        return Json(new { status = false, message = "Warning! Amount is Required", type = ActionResultTypeConstants.Message });

                    if (string.IsNullOrWhiteSpace(model.PaymentTrxId))
                        return Json(new { status = false, message = "Warning! Trx Id is Required", type = ActionResultTypeConstants.Message });

                }
                else if (model.PaymentType == CheckoutPaymentTypeConstants.Check_Or_Payorder)
                {
                    if (string.IsNullOrWhiteSpace(model.CheckInfo))
                        return Json(new { status = false, message = "Warning! Check info is Required", type = ActionResultTypeConstants.Message });

                    if (model.CheckOrPayorderAmount <= 0)
                        return Json(new { status = false, message = "Warning! Amount is Required", type = ActionResultTypeConstants.Message });

                    if (string.IsNullOrWhiteSpace(model.BankInfo))
                        return Json(new { status = false, message = "Warning! Bank Info is Required", type = ActionResultTypeConstants.Message });
                }

                //Populate Payment Complete Info
                var updateClassifiedText = PopulatePaymentCompleteInfo(model);

                //let's update ABPrintClassifiedText
                var isUpdated = await aBPrintClassifiedTextService.UpdatePaymentInfo(updateClassifiedText);

                if (!isUpdated)
                    return Json(new { status = false, message = "Error Occurred. Contact with support team.", type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/{model.MasterId}/Confirmation/{model.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [HttpGet]
        [Route("/checkout/classifiedtext/{id}/payment-complete/{bookingno}/card")]
        public async Task<IActionResult> PaymentCompleteWithCard(int id, string bookingno)
        {
            try
            {
                if (!CurrentLoginUser.IsCorrespondentUser)
                    return Redirect("/adbook/booknow");

                var bookingStep = BookingStepConstants.Checkout;
                var classifiedText = await aBPrintClassifiedTextService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

                if (classifiedText == null)
                    return Redirect("/adbook/booknow");

                var netPayable = Convert.ToInt32(Math.Round(classifiedText.NetAmount) + Math.Round(classifiedText.VATAmount) - Math.Round(classifiedText.Commission));

                var trxtId = $"CT-{bookingno}";

                //Populate payment gateway request
                var paymentGatewayRequest = PopulatePaymentGatewayRequest(id, bookingno, netPayable, trxtId);

                //let's initiate payment transaction
                var sslRedirectUrl = "";

                //Populate payment transaction
                var newSSLPaymentTransaction = PopulatePaymentTransaction(id, bookingno, netPayable, sslRedirectUrl);

                //let's add ssl payment gatway
                await sslPaymentTransactionService.Add(newSSLPaymentTransaction);

                var returnUrl = $"{sslRedirectUrl}";

                //let's track user info into system cache
                cacheManagerService.SetSSLKeysValue(bookingno, CurrentLoginUser.UserId);

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Checkout Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Ajax Calls

        private SSLPaymentTransaction PopulateSSLPaymentTransaction(int id, string adtype)
        {
            string tran_Id = Request.Form["tran_id"];
            string val_Id = Request.Form["val_id"];
            string amount = Request.Form["amount"];
            string cart_Type = Request.Form["cart_type"];
            string store_Amount = Request.Form["store_amount"];
            string Card_No = Request.Form["card_no"];
            string bank_Tran_Id = Request.Form["bank_tran_id"];
            string status = Request.Form["status"];
            string tran_Date = Request.Form["tran_date"];
            string error = Request.Form["error"];
            string currency = Request.Form["currency"];
            string card_Issuer = Request.Form["card_issuer"];
            string card_Brand = Request.Form["card_brand"];
            string card_Sub_Brand = Request.Form["card_sub_brand"];
            string card_Issuer_Country = Request.Form["card_issuer_country"];
            string card_Issuer_Country_Code = Request.Form["card_issuer_country_code"];
            string store_Id = Request.Form["store_id"];
            string verify_Sign = Request.Form["verify_sign"];
            string verify_Key = Request.Form["verify_key"];
            string verify_Sign_Sha2 = Request.Form["verify_sign_sha2"];
            string currency_Type = Request.Form["currency_type"];
            string currency_Amount = Request.Form["currency_amount"];
            string currency_Rate = Request.Form["currency_rate"];
            string base_Fair = Request.Form["base_fair"];
            string value_A = Request.Form["value_a"];
            string value_B = Request.Form["value_b"];
            string value_C = Request.Form["value_c"];
            string value_D = Request.Form["value_d"];
            string risk_Level = Request.Form["risk_level"];
            string risk_Title = Request.Form["risk_title"];

            var updateSSLPaymentTransaction = new SSLPaymentTransaction
            {
                AdType = adtype,
                AdMasterId = id,
                Tran_Id = tran_Id,
                Val_Id = val_Id,
                Amount = Convert.ToDecimal(amount),
                Cart_Type = cart_Type,
                Store_Amount = Convert.ToDecimal(string.IsNullOrWhiteSpace(store_Amount) ? "0" : store_Amount),
                Card_No = Card_No,
                Bank_Tran_Id = bank_Tran_Id,
                Status = status,
                Tran_Date = tran_Date,
                Error = error,
                Currency = currency,
                Card_Issuer = card_Issuer,
                Card_Brand = card_Brand,
                Card_Sub_Brand = card_Sub_Brand,
                Card_Issuer_Country = card_Issuer_Country,
                Card_Issuer_Country_Code = card_Issuer_Country_Code,
                Store_Id = store_Id,
                Verify_Sign = verify_Sign,
                Verify_Key = verify_Key,
                Verify_Sign_Sha2 = verify_Sign_Sha2,
                Currency_Type = currency_Type,
                Currency_Amount = currency_Amount,
                Currency_Rate = currency_Rate,
                Base_Fair = base_Fair,
                Value_A = value_A,
                Value_B = value_B,
                Value_C = value_C,
                Value_D = value_D,
                Risk_Level = risk_Level,
                Risk_Title = risk_Title,

                PaymentTransactionStatus = PaymentTransactionStatusConstants.Completed,
                FailedReason = null
            };
            return updateSSLPaymentTransaction;
        }

        private SSLPaymentTransaction PopulatePaymentTransaction(int adMasterId, string bookingno, int netPayable, string sslRedirectUrl)
        {
            return new SSLPaymentTransaction
            {
                AdType = AdTypeConstants.ClassifiedText,
                AdMasterId = adMasterId,
                Amount = netPayable,
                GatwayNote = $@"{AdTypeConstants.ClassifiedText} ad with {bookingno}. Total Amount {netPayable}",
                FailedReason = null,
                RedirectGatewayURL = sslRedirectUrl,
                PaymentTransactionStatus = PaymentTransactionStatusConstants.InProgress,
                CreatedBy = CurrentLoginUser.UserId,
            };
        }
        private PaymentGatewayRequest PopulatePaymentGatewayRequest(int id, string bookingno, decimal totalPrice, string labReceiptNo)
        {
            var adType = AdTypeConstants.ClassifiedText;
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var products = AdTypeConstants.ClassifiedText;
            var successUrl = $"{baseUrl}/checkout/{id}/confirmation/{bookingno}/mode/{CheckoutPaymentTypeConstants.Card}/adtype/{adType}";

            return new PaymentGatewayRequest
            {
                total_amount = totalPrice,
                tran_id = labReceiptNo,
                success_url = $"{successUrl}",
                fail_url = $"{baseUrl}/sslcommerce/failed/adtype/{adType}/{id}/bookingno/{bookingno}",
                cancel_url = $"{baseUrl}/sslcommerce/cancel/adtype/{adType}/{id}/bookingno/{bookingno}",
                version = "4.00",
                cus_name = "N/A",
                cus_email = "N/A",
                cus_add1 = "N/A",
                cus_add2 = "N/A",
                cus_city = "N/A",
                cus_state = "N/A",
                cus_postcode = "N/A",
                cus_country = "Bangladesh",
                cus_phone = "N/A",
                cus_fax = "N/A",
                ship_name = "N/A",
                ship_add1 = "N/A",
                ship_add2 = "N/A",
                ship_city = "N/A",
                ship_state = "N/A",
                ship_postcode = "N/A",
                ship_country = "Bangladesh",
                value_a = "N/A",
                value_b = "N/A",
                value_c = "N/A",
                value_d = "N/A",
                shipping_method = "NO",
                num_of_item = 0,
                product_name = $"{products}",
                product_profile = "general",
                product_category = "N/A",

                IsSandbox = Convert.ToBoolean(configuration["PaymentGateway:SSLCommerz:IsSandbox"]),
            };
        }

        [HttpPost]
        public async Task<JsonResult> GetOrderTotalAmount(OrderTotalCalculationViewModel model)
        {
            //get stored classified text from session
            var classifiedText = HttpContext.Session.GetObject<ABPrintClassifiedText>(SessionKeyConstants.ABPrintClassifiedText);
            if (classifiedText == null)
            {
                //get classified text from database
                classifiedText = await aBPrintClassifiedTextService.GetDetails(model.MasterTableId, model.BookingNo);
                if (classifiedText == null)
                    return Json(new { status = false, message = "Ad configuration not found. Please contact with support", type = ActionResultTypeConstants.Message });
            }

            var ratesClassifiedText = await ratePrintClassifiedTextService.GetClassifiedTextRatesByEditionIds(model.EditionIds);
            if (!ratesClassifiedText.Any())
                return Json(new { status = false, message = "Ad Rate not found. Please check at least one edition", type = ActionResultTypeConstants.Message });

            //Get order calculations
            model = await GetOrderCalculations(model);

            return Json(new
            {
                status = true,
                EstimatedTotalAmount = model.EstimatedTotalAmount,
                EditionDiscountPercentage = model.EditionDiscountPercentage,
                NetAmount = model.NetAmount,
                Commission = model.Commission,
                VATAmount = model.VATAmount,
                type = ActionResultTypeConstants.Message
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetManualDiscount(int paymentModeId)
        {
            if (paymentModeId <= 0)
                return Json(new { status = false, message = "Please select payment mode", type = ActionResultTypeConstants.Message });

            var manualDiscountPercentage = await GetDefaultDiscount(paymentModeId);

            return Json(new
            {
                status = true,
                ManualDiscountPercentage = manualDiscountPercentage,
                type = ActionResultTypeConstants.Message
            });
        }

        #endregion

        #region Private Methods
        private ABPrintClassifiedText PopulatePaymentCompleteInfo(PaymentViewModel model)
        {
            var updateClassifiedText = new ABPrintClassifiedText();
            if (model.PaymentType == CheckoutPaymentTypeConstants.Direct)
            {
                updateClassifiedText = new ABPrintClassifiedText
                {
                    ABPrintClassifiedTextId = model.MasterId,
                    PaymentType = model.PaymentType,
                    PaymentMethod = model.PaymentMethod,
                    PaymentMobileNumber = model.PaymentMobileNumber,
                    PaymentTrxId = model.PaymentTrxId,
                    PaymentPaidAmount = model.PaymentPaidAmount,
                    BookingStep = BookingStepConstants.Completed,
                    ModifiedBy = CurrentLoginUser.UserId
                };
            }
            else if (model.PaymentType == CheckoutPaymentTypeConstants.Check_Or_Payorder)
            {
                updateClassifiedText = new ABPrintClassifiedText
                {
                    ABPrintClassifiedTextId = model.MasterId,
                    PaymentType = model.PaymentType,
                    PaymentMethod = PaymentMethodConstants.Check_Or_Payorder,
                    PaymentMobileNumber = model.CheckInfo,
                    PaymentTrxId = model.BankInfo,
                    PaymentPaidAmount = model.CheckOrPayorderAmount,
                    BookingStep = BookingStepConstants.Completed,
                    ModifiedBy = CurrentLoginUser.UserId
                };
            }

            return updateClassifiedText;
        }
        private async Task<string> GenerateBillNo(int paymentModeId)
        {
            var billNo = string.Empty;
            try
            {
                int totalCount = 0; string billNumber = "";
                totalCount = await aBPrintClassifiedTextService.GetTotalCountByPaymentMode(paymentModeId);
                billNumber = GetBillNumber(totalCount);

                if (paymentModeId == PaymentModeConstants.Cash || paymentModeId == PaymentModeConstants.SSL)                
                    billNo = $@"AJP/CT/RPT/{billNumber}";                
                else                
                    billNo = $@"AJP/CT/CR/{billNumber}";                
            }
            catch
            {
                billNo = string.Empty;
            }

            return billNo;
        }

        private string GetBillNumber(int totalCount)
        {
            var billNumber = "";
            totalCount = totalCount + 1;
            switch (totalCount.ToString().Length)
            {
                case 1:
                    billNumber = $"0000{totalCount}";
                    break;
                case 2:
                    billNumber = $"000{totalCount}";
                    break;
                case 3:
                    billNumber = $"00{totalCount}";
                    break;
                case 4:
                    billNumber = $"0{totalCount}";
                    break;
                default:
                    billNumber = $"{totalCount}";
                    break;
            }

            return billNumber;
        }


        private async Task GetDefaultPaymentModeAndDiscount(CheckoutViewModel model)
        {
            var checkitem = await aBPrintClassifiedTextService.CheckAgency(model.ABPrintClassifiedTextId, model.BookingNo);

            if (CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User)
            {
                model.PaymentModeId = PaymentModeConstants.Cash;
                var manualDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);
                if(checkitem=="F")
                {
                    model.ManualDiscountPercentage = (int)manualDiscountPercentage;
                }
                else
                {
                    model.ManualDiscountPercentage = 0;
                }
                
            }
            else if (CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
            {
                model.PaymentModeId = PaymentModeConstants.SSL;
                var manualDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);
                model.ManualDiscountPercentage = 0;
            }
        }

        private async Task<decimal> GetDefaultDiscount(int paymentModeId)
        {
            decimal manualDiscountPercentage = 0;
            //get default discount 
            var defaultDiscount = await defaultDiscountService
                .GetDetailsByAdTypeAndPaymentMode(ClassifiedTypeConstants.ClassifiedText, paymentModeId);

            if (defaultDiscount != null)
                manualDiscountPercentage = defaultDiscount.DiscountRate;

            return manualDiscountPercentage;
        }

        private void ClearSessionData()
        {
            HttpContext.Session.Remove(SessionKeyConstants.ABPrintClassifiedText);
            HttpContext.Session.Remove(SessionKeyConstants.RatePrintClassifiedTextByEditions);
            HttpContext.Session.Remove(SessionKeyConstants.RatePrintClassifiedText);
        }

        private decimal GetNetAmount(OrderTotalCalculationViewModel model)
        {
            decimal netAmount = 0;
            //let's calculate net amount 
            netAmount = model.EstimatedTotalAmount - ((model.EstimatedTotalAmount * model.TotalDiscountPercentages) / 100);

            return netAmount;
        }

        private decimal GetOrderTotalAmount(ABPrintClassifiedText classifiedText, IEnumerable<RatePrintClassifiedText> ratesClassifiedText)
        {
            decimal orderTotalAmount = 0;
            foreach (var rate in ratesClassifiedText)
            {
                decimal baseAmount = classifiedText.TotalCount * rate.PerWordRate;
                decimal aditionalAmountBullet = 0;
                decimal aditionalAmount = 0;

                //for bullet related
                if (classifiedText.IsBigBulletPointSingle)
                {
                    aditionalAmountBullet = (baseAmount * rate.BigBulletPointSingle) / 100;
                }
                if (classifiedText.IsBigBulletPointDouble)
                {
                    aditionalAmountBullet = (baseAmount * rate.BigBulletPointDouble) / 100;
                }

                //ad enhancetype wise 
                if (classifiedText.IsBold)
                {
                    aditionalAmount = (baseAmount * rate.BoldPercentage) / 100;
                }
                if (classifiedText.IsBoldinScreen)
                {
                    aditionalAmount = (baseAmount * rate.BoldinScreenPercentage) / 100;
                }
                if (classifiedText.IsBoldScreenSingleBox)
                {
                    aditionalAmount = (baseAmount * rate.BoldScreenSingleBoxPercentage) / 100;
                }
                if (classifiedText.IsBoldScreenDoubleBox)
                {
                    aditionalAmount = (baseAmount * rate.BoldScreenDoubleBoxPercentage) / 100;
                }

                orderTotalAmount = orderTotalAmount + (baseAmount + aditionalAmountBullet + aditionalAmount);
            }

            return orderTotalAmount;
        }

        private string GetAdEnhancementAndAdContent(ABPrintClassifiedText classifiedText)
        {
            string adEnhancementBullet = string.Empty;
            string adEnhancement = string.Empty;
            string adConent = classifiedText.AdContent;
            //for bullet related
            if (classifiedText.IsBigBulletPointSingle)
            {
                adEnhancementBullet = AddEnhancementTypeConstants.BigBulletPointSingle;
                adConent = $@"🞇 {classifiedText.AdContent}";
            }
            if (classifiedText.IsBigBulletPointDouble)
            {
                adEnhancementBullet = AddEnhancementTypeConstants.BigBulletPointDouble;
                adConent = $@"🞇🞇 {classifiedText.AdContent}";
            }

            if (!string.IsNullOrWhiteSpace(adEnhancementBullet))
                adEnhancementBullet = $@"<span><i class='fa fa-check-square-o' aria-hidden='true'></i> {adEnhancementBullet}</span> ";

            //ad enhancetype wise 
            if (classifiedText.IsBold)
            {
                adEnhancement = AddEnhancementTypeConstants.Bold;
                adConent = $@"<div style='font-weight:bold;'>{adConent}</div>";
            }
            if (classifiedText.IsBoldinScreen)
            {
                adEnhancement = AddEnhancementTypeConstants.BoldInScreen;
                adConent = $@"<div style='font-weight:bold; background-color:gray;'>{adConent}</div>";
            }
            if (classifiedText.IsBoldScreenSingleBox)
            {
                adEnhancement = AddEnhancementTypeConstants.BoldInScreenAndSingleBox;
                adConent = $@"<div style='font-weight:bold; background-color:gray;border: 2px solid black;'>{adConent}</div>";
            }
            if (classifiedText.IsBoldScreenDoubleBox)
            {
                adEnhancement = AddEnhancementTypeConstants.BoldInScreenAndDoubleBoxes;

                adConent = $@"  <div style='border: 2px solid black; margin:1px;padding: 2px'>
                                    <div style='font-weight:bold; background-color:gray;border: 2px solid black;'>
                                        {adConent}
                                    </div>
                                </div>
                                ";
            }

            classifiedText.AdContent = adConent;

            if (!string.IsNullOrWhiteSpace(adEnhancement))
                adEnhancement = $@"<span><i class='fa fa-check-square-o' aria-hidden='true'></i> {adEnhancement}</span>";

            var finalAdEnhancement = $@"{adEnhancementBullet} {adEnhancement}";

            return finalAdEnhancement;
        }

        private async Task<OrderTotalCalculationViewModel> GetOrderCalculations(OrderTotalCalculationViewModel model)
        {
            //get stored classified text from session
            var classifiedText = HttpContext.Session.GetObject<ABPrintClassifiedText>(SessionKeyConstants.ABPrintClassifiedText);
            if (classifiedText == null)
            {
                //get classified text from database
                classifiedText = await aBPrintClassifiedTextService.GetDetails(model.MasterTableId, model.BookingNo);
                if (classifiedText == null)
                    return model;
            }

            //get total publish dates 
            int totalPublishDates = HttpContext.Session.GetInt(SessionKeyConstants.ClassifiedTextTotalPublishDates);
            if (totalPublishDates <= 0)
            {
                var classifiedTextDetailListing = await aBPrintClassifiedTextService.GetABPrintClassifiedTextDetailListing(model.MasterTableId);

                if (!classifiedTextDetailListing.Any()) totalPublishDates = 0;

                if (classifiedTextDetailListing.Any())
                {
                    var basedOfferDates = classifiedTextDetailListing.Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();
                    totalPublishDates = basedOfferDates.Count();
                }
            }

            //get rates by eiditions
            var ratesClassifiedText = await ratePrintClassifiedTextService.GetClassifiedTextRatesByEditionIds(model.EditionIds);
            if (!ratesClassifiedText.Any())
                return model;

            //get rates by default eidtion or national edition exist in editions list            
            if (model.NationalEdition)
            {
                ratesClassifiedText = ratesClassifiedText.Where(f => f.EditionId == EditionConstants.National);
                if (!ratesClassifiedText.Any())
                    return model;
            }

            //Get order total amount
            model.EstimatedTotalAmount = GetOrderTotalAmount(classifiedText, ratesClassifiedText);
            model.EstimatedTotalAmount = model.EstimatedTotalAmount * Convert.ToDecimal(totalPublishDates);

            //if national edition not found then fetch edition offer
            if (!model.NationalEdition)
            {
                int noofOfferEditions = model.EditionIds.Count - 1;

                var editionOffer = await offerEditionService.GetByNoofEdition(noofOfferEditions);
                if (editionOffer != null && editionOffer.EditionDateId > 0)
                {
                    model.OfferEditionId = editionOffer.EditionDateId;
                    model.EditionDiscountPercentage = editionOffer.DiscountPercentage;
                }
            }

            var dateOfferPercentage = classifiedText.DiscountPercent;
            var manualDiscountPercentage = model.ManualDiscountPercentage;
           
            model.TotalDiscountPercentages = dateOfferPercentage + model.EditionDiscountPercentage + manualDiscountPercentage;

            //Get net amount
            model.NetAmount = GetNetAmount(model);

            model.Commission = 0;
            if (CurrentLoginUser.IsCorrespondentUser)
                model.Commission = (model.NetAmount * CurrentLoginUser.DefaultCommission) / 100;

            //agency commission
            if (!CurrentLoginUser.IsCorrespondentUser && classifiedText.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsById((int)classifiedText.AgencyId);
                if (agency != null && agency.AgencyId > 0)
                    model.Commission = (model.NetAmount * agency.CTCommission) / 100;
            }

            decimal vat = 0;
            if (model.NationalEdition) vat = 15;
            model.VATAmount = ((model.NetAmount - model.Commission) * vat) / 100;

            return model;
        }

        private async Task<ABPrintClassifiedText> RegenerateABPrintClassifiedTextInfo(CheckoutViewModel model)
        {
            var defaultDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);

            //get stored classified text from session
            var classifiedText = HttpContext.Session.GetObject<ABPrintClassifiedText>(SessionKeyConstants.ABPrintClassifiedText);

            var orderTotalCalculationModel = new OrderTotalCalculationViewModel
            {
                EditionIds = model.EditionIds,
                MasterTableId = classifiedText.ABPrintClassifiedTextId,
                BookingNo = classifiedText.BookingNo,
                NationalEdition = model.NationalEdition,
                ManualDiscountPercentage = model.ManualDiscountPercentage
            };

            //Get order calculations
            var orderTotalCalculation = await GetOrderCalculations(orderTotalCalculationModel);

            var newABPrintClassifiedText = new ABPrintClassifiedText
            {
                ABPrintClassifiedTextId = classifiedText.ABPrintClassifiedTextId,
                BookingNo = classifiedText.BookingNo,
                NationalEdition = model.NationalEdition,
                BookingStep = BookingStepConstants.Checkout,
                AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                GrossTotal = Math.Round(orderTotalCalculation.EstimatedTotalAmount, 0, MidpointRounding.AwayFromZero),
                DiscountPercent = model.ManualDiscountPercentage,
                OfferEditionId = orderTotalCalculation.OfferEditionId,
                PaymentModeId = model.PaymentModeId,
                BillNo = await GenerateBillNo(model.PaymentModeId),
                BillDate = Convert.ToDateTime(model.BillDate),
                PaymentStatus = PaymentStatusConstants.Not_Paid,                
                ModifiedBy = CurrentLoginUser.UserId
            };

            newABPrintClassifiedText.DiscountAmount = Math.Round(((newABPrintClassifiedText.GrossTotal * orderTotalCalculation.TotalDiscountPercentages) / 100), 0, MidpointRounding.AwayFromZero);
            newABPrintClassifiedText.NetAmount = Math.Round(orderTotalCalculation.NetAmount, 0, MidpointRounding.AwayFromZero);

            newABPrintClassifiedText.Commission = Math.Round(orderTotalCalculation.Commission, 0, MidpointRounding.AwayFromZero);
            newABPrintClassifiedText.VATAmount = Math.Round(orderTotalCalculation.VATAmount, 0, MidpointRounding.AwayFromZero);

            //approval status
            newABPrintClassifiedText.ApprovalStatus =
                (model.ManualDiscountPercentage <= defaultDiscountPercentage)
                ? newABPrintClassifiedText.ApprovalStatus = ApproveStatusConstants.Approved
                : newABPrintClassifiedText.ApprovalStatus = ApproveStatusConstants.Pending_Approval_Layer1;

            return newABPrintClassifiedText;
        }

        private async Task<List<ABPrintClassifiedTextDetail>> PopulateABPrintClassifiedTextDetail(CheckoutViewModel model)
        {
            var listing = new List<ABPrintClassifiedTextDetail>();
            int pageNumber = EditionPagesConstants.Page_6; //this should be dynamic from ui

            var editionPages = await editionPageService.GetEditionPagesByEditionAndPageNo(model.EditionIds, pageNumber);

            foreach (var editionId in model.EditionIds)
            {
                var editionPage = editionPages.FirstOrDefault(a => a.EditionId == editionId && a.EditionPageNo == pageNumber);

                var newClassifiedTextDetail = new ABPrintClassifiedTextDetail
                {
                    EditionId = editionId,
                    EditionPageId = editionPage?.EditionPageId
                };

                listing.Add(newClassifiedTextDetail);
            }
            return listing;
        }

        #endregion
    }
}
