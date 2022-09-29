using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Service.ABPrintClassifiedDisplays;
using pbAd.Service.Advertisers;
using pbAd.Service.Agencies;
using pbAd.Service.CacheManagerServices;
using pbAd.Service.Categories;
using pbAd.Service.DefaultDiscounts;
using pbAd.Service.EditionDistricts;
using pbAd.Service.EditionPages;
using pbAd.Service.Editions;
using pbAd.Service.OfferEditions;
using pbAd.Service.PaymentGateways;
using pbAd.Service.PaymentGateways.Models;
using pbAd.Service.RatePrintClassifiedDisplays;
using pbAd.Service.SSLPaymentTransactions;
using pbAd.Service.SubCategories;
using pbAd.Web.Infrastructure.Helpers;
using pbAd.Web.ViewModels.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pbAd.Web.Controllers
{
    public class CheckoutClassifiedDisplayController : AdminBaseController
    {
        #region Private Methods
        private readonly IEditionService editionService;
        private readonly IABPrintClassifiedDisplayService abPrintClassifiedDisplayService;
        private readonly IRatePrintClassifiedDisplayService ratePrintClassifiedDisplayService;
        private readonly IDefaultDiscountService defaultDiscountService;
        private readonly IAgencyService agencyService;
        private readonly IEditionPageService editionPageService;
        private readonly IOfferEditionService offerEditionService;
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;
        private readonly ISSLPaymentTransactionService sslPaymentTransactionService;
       
        private readonly IConfiguration configuration;
        private readonly IEditionDistrictService editionDistrictService;
        private readonly IAdvertiserService advertiserService;
        private readonly ICacheManagerService cacheManagerService;

        #endregion

        #region Ctor

        public CheckoutClassifiedDisplayController(IEditionService editionService,
                IABPrintClassifiedDisplayService abPrintClassifiedDisplayService,
                IDefaultDiscountService defaultDiscountService,
                IOfferEditionService offerEditionService,
                IAgencyService agencyService,
                ICategoryService categoryService,
                ISubCategoryService subCategoryService,
                IAdvertiserService advertiserService,
                IEditionPageService editionPageService,
             
                IConfiguration configuration,
                ISSLPaymentTransactionService sslPaymentTransactionService,
                IEditionDistrictService editionDistrictService,
                ICacheManagerService cacheManagerService,
                IRatePrintClassifiedDisplayService ratePrintClassifiedDisplayService
            )
        {
            this.editionService = editionService;
            this.offerEditionService = offerEditionService;
            this.abPrintClassifiedDisplayService = abPrintClassifiedDisplayService;
            this.ratePrintClassifiedDisplayService = ratePrintClassifiedDisplayService;
            this.defaultDiscountService = defaultDiscountService;
            this.agencyService = agencyService;
            this.editionPageService = editionPageService;
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
           
            this.agencyService = agencyService;
            this.configuration = configuration;
            this.sslPaymentTransactionService = sslPaymentTransactionService;
            this.advertiserService = advertiserService;
            this.cacheManagerService = cacheManagerService;
            this.editionDistrictService = editionDistrictService;
        }

        #endregion

        #region Payment

        [Route("/checkout/classifieddisplay/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(int id, string bookingno)
        {
            var bookingStep = BookingStepConstants.Booked;
            var classifiedDisplay = await abPrintClassifiedDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);
            if (classifiedDisplay == null)
                return Redirect("/bookclasssifieddisplayad/booknow");

            var classifiedDisplayDetailListing = await abPrintClassifiedDisplayService.GetABPrintClassifiedDisplayDetailListing(id);

            if (!classifiedDisplayDetailListing.Any())
                return Redirect("/bookclasssifieddisplayad/booknow");

            var edtions = await editionService.GetAllEditions();
            var defaultEdtion = edtions.FirstOrDefault(f => f.EditionId == CurrentLoginUser.EditionId);
            var filteredEditions = edtions.Where(f => f.EditionId != EditionConstants.National).ToList();

            var basedOfferDates = classifiedDisplayDetailListing.OrderBy(o => o.PublishDate).Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();
            var basedOfferDatesInString = String.Join(", ", basedOfferDates);
            string adContent = "", title = "", finalImageUrl = "~/img/display-add-default-image.jpg";

            var abPrintClassifiedDisplayDetail = classifiedDisplayDetailListing.FirstOrDefault();
            if (abPrintClassifiedDisplayDetail != null)
            {
                title = abPrintClassifiedDisplayDetail.Title;
                adContent = abPrintClassifiedDisplayDetail.AdContent;
                finalImageUrl = abPrintClassifiedDisplayDetail.FinalImageUrl;
            }

            var editionDistrictList = await editionDistrictService.GetAllEditionDistricts();
            var editionDistrict = editionDistrictList.FirstOrDefault(f => f.EditionId == defaultEdtion.EditionId);
            var defaultEditionToolTip = "";
            if (editionDistrict != null)
                defaultEditionToolTip = editionDistrict.HoverTextValue;

            //rate
            var editionId = CurrentLoginUser.EditionId ?? 0;
            var rateClassifiedDisplay = await ratePrintClassifiedDisplayService.GetDefaultRatePrintClassifiedDisplay(editionId);

            //Get Classified Display Ad Rate
            decimal adRate = GetClassifiedDisplayAdRate(classifiedDisplay.AdColumnInch, rateClassifiedDisplay);

            var model = new CheckoutClassifiedDisplayViewModel
            {
                ABPrintClassifiedDisplay = classifiedDisplay,
                Editions = filteredEditions,
                BasedOfferDatesInString = basedOfferDatesInString,
                ABPrintClassifiedDisplayId = id,
                BookingNo = bookingno,
                DefaultEditionToolTip = defaultEditionToolTip,

                DefaultEditionName = defaultEdtion.EditionName,
                DefaultEditionId = defaultEdtion.EditionId,
                EditionDistrictList = editionDistrictList,
                ManualDiscountPercentage = 0,
                FinalImageUrl = finalImageUrl,
                PaymentModeId = classifiedDisplay.AgencyId > 0 ? PaymentModeConstants.Credit : PaymentModeConstants.Cash,
                Rate = adRate,
                Remarks = classifiedDisplay.Remarks,
                BillDate = basedOfferDates.FirstOrDefault()
            };

            //Populate Category SubCategory Agency and Advertiser Info
            await PopulateCategorySubCategoryAgencyAdvertiserInfo(classifiedDisplay, model);

            //Get default payment mode & discount
            await GetDefaultPaymentModeAndDiscount(model);

            classifiedDisplay.Title = title;
            classifiedDisplay.AdContent = adContent;

            //store classified text into session
            HttpContext.Session.SetObject(SessionKeyConstants.ABPrintClassifiedDisplay, classifiedDisplay);
            HttpContext.Session.SetInt(SessionKeyConstants.ClassifiedDisplayTotalPublishDates, basedOfferDates.Count());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/classifieddisplay/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(CheckoutClassifiedDisplayViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                if (!model.AcceptTermsAndConditions)
                    return Json(new { status = false, message = "Please accept terms and conditions", type = ActionResultTypeConstants.Message });

                //get stored classified text from session
                var classifiedDisplay = HttpContext.Session.GetObject<ABPrintClassifiedDisplay>(SessionKeyConstants.ABPrintClassifiedDisplay);

                if (classifiedDisplay == null)
                    return Json(new { status = false, message = "Warning! Classified Display Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                if ((classifiedDisplay.ABPrintClassifiedDisplayId != model.ABPrintClassifiedDisplayId)
                    || (classifiedDisplay.BookingNo != model.BookingNo))
                    return Json(new { status = false, message = "Warning! Classified Display Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                if (model.NationalEdition)
                {
                    var defaultEditionId = EditionConstants.National;
                    model.EditionIds = new List<int>();
                    model.EditionIds.Add(defaultEditionId);
                }

                //re-generate ab print classified display
                var updateClassifiedDisplay = await RegenerateABPrintClassifiedDisplayInfo(model);

                //Populate classified display detail
                var classifiedDisplayDetailListing = await PopulateClassifiedDisplayDetail(model);

                var checkProcessRequest = new CheckoutClassfiedDisplayProcessModel
                {
                    ABPrintClassifiedDisplayId = model.ABPrintClassifiedDisplayId,
                    ABPrintClassifiedDisplay = updateClassifiedDisplay,
                    ABPrintClassifiedDisplayDetailListing = classifiedDisplayDetailListing
                };

                //let's Add ABPrintClassifiedDisplay Detail
                var response = await abPrintClassifiedDisplayService.CheckoutClassifiedDisplayProcess(checkProcessRequest);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/classifieddisplay/{model.ABPrintClassifiedDisplayId}/Confirmation/{model.BookingNo}";

                if (CurrentLoginUser.IsCorrespondentUser && model.PaymentModeId == PaymentModeConstants.SSL)
                    returnUrl = $"/checkout/classifieddisplay/{model.ABPrintClassifiedDisplayId}/payment-complete/{model.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [AllowAnonymous]
        [Route("/checkout/classifieddisplay/{id}/confirmation/{bookingno}")]
        [Route("/checkout/classifieddisplay/{id}/confirmation/{bookingno}/mode/{paymentmode}")]
        [Route("/checkout/classifieddisplay/{id}/confirmation/{bookingno}/mode/{paymentmode}/adtype/{adtype}")]
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

                    var updateABPrintClassifiedDisplay = new ABPrintClassifiedDisplay
                    {
                        ABPrintClassifiedDisplayId = id,
                        ApprovalStatus = ApproveStatusConstants.Approved,
                        PaymentStatus = PaymentStatusConstants.Paid,
                        AdStatus = AdStatusConstants.Confirmed_And_Paid,
                        ActualReceived = updateSSLPaymentTransaction.Amount,
                        PaymentTrxId = updateSSLPaymentTransaction.Bank_Tran_Id,
                        PaymentModeId = paymentmode,
                        MoneyReceiptNo = updateSSLPaymentTransaction.Bank_Tran_Id,
                        PaymentMobileNumber = PaymentModeConstants.GetText(PaymentModeConstants.SSL),
                        ModifiedBy = updatedBy
                    };

                    //let's update payment mode
                    await abPrintClassifiedDisplayService.UpdatePaymentMode(updateABPrintClassifiedDisplay);

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

        [Route("/checkout/classifieddisplay/{id}/payment-complete/{bookingno}")]
        public async Task<IActionResult> PaymentComplete(int id, string bookingno)
        {
            if (!CurrentLoginUser.IsCorrespondentUser)
                return Redirect("/bookclasssifieddisplayad/booknow");

            var bookingStep = BookingStepConstants.Checkout;
            var classifiedDisplay = await abPrintClassifiedDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

            if (classifiedDisplay == null)
                return Redirect("/bookclasssifieddisplayad/booknow");

            var netPayable = Convert.ToInt32(Math.Round(classifiedDisplay.NetAmount) + Math.Round(classifiedDisplay.VATAmount) - Math.Round(classifiedDisplay.Commission));

            var model = new PaymentViewModel
            {
                MasterId = classifiedDisplay.ABPrintClassifiedDisplayId,
                BookingNo = bookingno,
                NetPayable = netPayable
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/classifieddisplay/{id}/payment-complete/{bookingno}")]
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
                var updateClassifiedDisplay = PopulatePaymentCompleteInfo(model);

                //let's update ABPrintClassifiedDisplay
                var isUpdated = await abPrintClassifiedDisplayService.UpdatePaymentInfo(updateClassifiedDisplay);

                if (!isUpdated)
                    return Json(new { status = false, message = "Error Occurred. Contact with support team.", type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/classifieddisplay/{model.MasterId}/Confirmation/{model.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [HttpGet]
        [Route("/checkout/classifieddisplay/{id}/payment-complete/{bookingno}/card")]
        public async Task<IActionResult> PaymentCompleteWithCard(int id, string bookingno)
        {
            try
            {
                if (!CurrentLoginUser.IsCorrespondentUser)
                    return Redirect("/bookprivatedisplayad/booknow");

                var bookingStep = BookingStepConstants.Checkout;
                var privateDisplay = await abPrintClassifiedDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

                if (privateDisplay == null)
                    return Redirect("/bookprivatedisplayad/booknow");

                var netPayable = Convert.ToInt32(Math.Round(privateDisplay.NetAmount) + Math.Round(privateDisplay.VATAmount) - Math.Round(privateDisplay.Commission));

                var trxtId = $"CD-{bookingno}";

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

        [HttpPost]
        public async Task<JsonResult> GetOrderTotalAmount(OrderTotalCalculationViewModel model)
        {
            //get stored classified text from session
            var classifiedDisplay = HttpContext.Session.GetObject<ABPrintClassifiedDisplay>(SessionKeyConstants.ABPrintClassifiedDisplay);
            if (classifiedDisplay == null)
            {
                //get classified display from database
                classifiedDisplay = await abPrintClassifiedDisplayService.GetDetailsByIdAndBookingNo(model.MasterTableId, model.BookingNo);
                if (classifiedDisplay == null)
                    return Json(new { status = false, message = "Ad configuration not found. Please contact with support", type = ActionResultTypeConstants.Message });
            }

            //get classified display rates by edition ids
            var ratesClassifiedDisplay = await ratePrintClassifiedDisplayService.GetClassifiedDisplayRatesByEditionIds(model.EditionIds);
            if (!ratesClassifiedDisplay.Any())
                return Json(new { status = false, message = "Ad Rate not found. Please contact with support", type = ActionResultTypeConstants.Message });

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
                AdType = AdTypeConstants.ClassifiedDisplay,
                AdMasterId = adMasterId,
                Amount = netPayable,
                GatwayNote = $@"{AdTypeConstants.ClassifiedDisplay} ad with {bookingno}. Total Amount {netPayable}",
                FailedReason = null,
                RedirectGatewayURL = sslRedirectUrl,
                PaymentTransactionStatus = PaymentTransactionStatusConstants.InProgress,
                CreatedBy = CurrentLoginUser.UserId,
            };
        }

        private PaymentGatewayRequest PopulatePaymentGatewayRequest(int id, string bookingno, decimal totalPrice, string labReceiptNo)
        {
            var adType = AdTypeConstants.ClassifiedDisplay;
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var products = AdTypeConstants.ClassifiedDisplay;
            var successUrl = $"{baseUrl}/checkout/classifieddisplay/{id}/confirmation/{bookingno}/mode/{CheckoutPaymentTypeConstants.Card}/adtype/{adType}";

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

        private ABPrintClassifiedDisplay PopulatePaymentCompleteInfo(PaymentViewModel model)
        {
            var updateClassifiedDisplay = new ABPrintClassifiedDisplay();
            if (model.PaymentType == CheckoutPaymentTypeConstants.Direct)
            {
                updateClassifiedDisplay = new ABPrintClassifiedDisplay
                {
                    ABPrintClassifiedDisplayId = model.MasterId,
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
                updateClassifiedDisplay = new ABPrintClassifiedDisplay
                {
                    ABPrintClassifiedDisplayId = model.MasterId,
                    PaymentType = model.PaymentType,
                    PaymentMethod = PaymentMethodConstants.Check_Or_Payorder,
                    PaymentMobileNumber = model.CheckInfo,
                    PaymentTrxId = model.BankInfo,
                    PaymentPaidAmount = model.CheckOrPayorderAmount,
                    BookingStep = BookingStepConstants.Completed,
                    ModifiedBy = CurrentLoginUser.UserId
                };
            }

            return updateClassifiedDisplay;
        }

        private async Task<string> GenerateBillNo(int paymentModeId)
        {
            var billNo = string.Empty;
            try
            {
                int totalCount = 0; string billNumber = "";
                totalCount = await abPrintClassifiedDisplayService.GetTotalCountByPaymentMode(paymentModeId);
                billNumber = GetBillNumber(totalCount);

                if (paymentModeId == PaymentModeConstants.Cash || paymentModeId == PaymentModeConstants.SSL)                
                    billNo = $@"AJP/CD/RPT/{billNumber}";                
                else                
                    billNo = $@"AJP/CD/RPT/{billNumber}";                
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


        private async Task PopulateCategorySubCategoryAgencyAdvertiserInfo(ABPrintClassifiedDisplay classifiedDisplay, CheckoutClassifiedDisplayViewModel model)
        {
            var category = await categoryService.GetDetailsById(classifiedDisplay.CategoryId);
            if (category != null)
                model.CategoryName = category.CategoryName;

            var subCategory = await subCategoryService.GetDetailsById(classifiedDisplay.SubCategoryId);
            if (subCategory != null)
                model.SubCategoryName = subCategory.SubCategoryName;

            if (classifiedDisplay.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsById((int)classifiedDisplay.AgencyId);
                if (agency != null)
                {
                    model.AgencyName = agency.AgencyName;
                    model.AgencyId = agency.AgencyId;
                    model.CollectorId = agency.CollectorId;
                }
            }

            if (classifiedDisplay.AdvertiserId > 0)
            {
                var advertiser = await advertiserService.GetDetailsById((int)classifiedDisplay.AdvertiserId);
                if (advertiser != null)
                {
                    model.AdvertiserName = advertiser.AdvertiserName;
                    model.AdvertiserMobileNo = advertiser.AdvertiserMobileNo;
                }
            }
        }
        private async Task<List<ABPrintClassifiedDisplayDetail>> PopulateClassifiedDisplayDetail(CheckoutClassifiedDisplayViewModel model)
        {
            var listing = new List<ABPrintClassifiedDisplayDetail>();
            var edtionPageNo = EditionPagesConstants.Page_7;
            var editionPages = await editionPageService.GetEditionPagesByEditionAndPageNo(model.EditionIds, edtionPageNo);

            foreach (var editionId in model.EditionIds)
            {
                var edtionPage = editionPages.FirstOrDefault(f => f.EditionId == editionId && f.EditionPageNo == edtionPageNo);

                var newClassifiedDisplayDetail = new ABPrintClassifiedDisplayDetail
                {
                    EditionId = editionId,
                    EditionPageId = edtionPage?.EditionPageId
                };

                listing.Add(newClassifiedDisplayDetail);
            }
            return listing;
        }

        private async Task GetDefaultPaymentModeAndDiscount(CheckoutClassifiedDisplayViewModel model)
        {
            /*
            if (CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User)
            {
                model.PaymentModeId = PaymentModeConstants.Cash;
                model.ManualDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);
            }
            else if (CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
            {
                model.PaymentModeId = PaymentModeConstants.SSL;
                model.ManualDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);
            }
            */

            var manualDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            int userGroup = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "GroupId").Value);

            if (userGroup == 1)
            {
                model.ManualDiscountPercentage = 0;
            }
            else
            {
                model.ManualDiscountPercentage = (int)(manualDiscountPercentage);
            }
                
        }

        private async Task<decimal> GetDefaultDiscount(int paymentModeId)
        {
            decimal manualDiscountPercentage = 0;
            //get default discount 
            var defaultDiscount = await defaultDiscountService
                .GetDetailsByAdTypeAndPaymentMode(ClassifiedTypeConstants.ClassifiedDisplay, paymentModeId);

            if (defaultDiscount != null)
                manualDiscountPercentage = defaultDiscount.DiscountRate;

            return manualDiscountPercentage;
        }

        private async Task<ABPrintClassifiedDisplay> RegenerateABPrintClassifiedDisplayInfo(CheckoutClassifiedDisplayViewModel model)
        {
            //get default discount
            var defaultDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);

            //get stored classified display from session
            var classifiedDisplay = HttpContext.Session.GetObject<ABPrintClassifiedDisplay>(SessionKeyConstants.ABPrintClassifiedDisplay);

            var orderTotalCalculationModel = new OrderTotalCalculationViewModel
            {
                EditionIds = model.EditionIds,
                MasterTableId = classifiedDisplay.ABPrintClassifiedDisplayId,
                BookingNo = classifiedDisplay.BookingNo,
                NationalEdition = model.NationalEdition,
                ManualDiscountPercentage = model.ManualDiscountPercentage
            };

            //Get order calculations
            var orderTotalCalculation = await GetOrderCalculations(orderTotalCalculationModel);

            var newABPrintClassifiedDisplay = new ABPrintClassifiedDisplay
            {
                ABPrintClassifiedDisplayId = classifiedDisplay.ABPrintClassifiedDisplayId,
                BookingNo = classifiedDisplay.BookingNo,
                BookingStep = BookingStepConstants.Checkout,
                AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                GrossTotal = Math.Round(orderTotalCalculation.EstimatedTotalAmount, 0, MidpointRounding.AwayFromZero),
                DiscountPercent = model.ManualDiscountPercentage,
                OfferEditionId = orderTotalCalculation.OfferEditionId,
                PaymentModeId = model.PaymentModeId,
                PaymentStatus = PaymentStatusConstants.Not_Paid,               
                BillNo = await GenerateBillNo(model.PaymentModeId),
                BillDate = Convert.ToDateTime(model.BillDate),
                NationalEdition = model.NationalEdition,
                ModifiedBy = CurrentLoginUser.UserId
            };

            newABPrintClassifiedDisplay.DiscountAmount = Math.Round((newABPrintClassifiedDisplay.GrossTotal * orderTotalCalculation.TotalDiscountPercentages) / 100, 0, MidpointRounding.AwayFromZero);
            newABPrintClassifiedDisplay.NetAmount = Math.Round(orderTotalCalculation.NetAmount, 0, MidpointRounding.AwayFromZero);

            newABPrintClassifiedDisplay.Commission = Math.Round(orderTotalCalculation.Commission, 0, MidpointRounding.AwayFromZero);
            newABPrintClassifiedDisplay.VATAmount = Math.Round(orderTotalCalculation.VATAmount, 0, MidpointRounding.AwayFromZero);

            //approval status
            //newABPrintClassifiedDisplay.ApprovalStatus =(model.ManualDiscountPercentage <= defaultDiscountPercentage)
            //    ? newABPrintClassifiedDisplay.ApprovalStatus = ApproveStatusConstants.Approved
            //    : newABPrintClassifiedDisplay.ApprovalStatus = ApproveStatusConstants.Pending_Approval_Layer1;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            int userGroup = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "GroupId").Value);

            if (userGroup==1)
            {
                newABPrintClassifiedDisplay.ApprovalStatus = ApproveStatusConstants.Approved;
            }
            else
            {
                if (model.ManualDiscountPercentage <= defaultDiscountPercentage)
                {
                    newABPrintClassifiedDisplay.ApprovalStatus = ApproveStatusConstants.Approved;
                }
                else
                {
                    newABPrintClassifiedDisplay.ApprovalStatus = ApproveStatusConstants.Pending_Approval_Layer1;
                }
            }

           

            if (model.PaymentModeId == PaymentModeConstants.Credit && model.AgencyId > 0)
                newABPrintClassifiedDisplay.CollectorId = model.CollectorId;

            return newABPrintClassifiedDisplay;
        }

        private async Task<OrderTotalCalculationViewModel> GetOrderCalculations(OrderTotalCalculationViewModel model)
        {
            //get stored classified text from session
            var classifiedDisplay = HttpContext.Session.GetObject<ABPrintClassifiedDisplay>(SessionKeyConstants.ABPrintClassifiedDisplay);
            if (classifiedDisplay == null)
            {
                //get classified text from database
                classifiedDisplay = await abPrintClassifiedDisplayService.GetDetailsByIdAndBookingNo(model.MasterTableId, model.BookingNo);
                if (classifiedDisplay == null)
                    return model;
            }

            //get total publish dates 
            int totalPublishDates = HttpContext.Session.GetInt(SessionKeyConstants.ClassifiedTextTotalPublishDates);
            if (totalPublishDates <= 0)
            {
                var classifiedDisplayDetailListing = await abPrintClassifiedDisplayService.GetABPrintClassifiedDisplayDetailListing(model.MasterTableId);

                if (!classifiedDisplayDetailListing.Any()) totalPublishDates = 0;

                if (classifiedDisplayDetailListing.Any())
                {
                    var basedOfferDates = classifiedDisplayDetailListing.Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();
                    totalPublishDates = basedOfferDates.Count();
                }
            }

            //get rates by eiditions
            var ratesClassifiedDisplay = await ratePrintClassifiedDisplayService.GetClassifiedDisplayRatesByEditionIds(model.EditionIds);
            if (!ratesClassifiedDisplay.Any())
                return model;

            //get rates by default eidtion or national edition exist in editions list            
            if (model.NationalEdition)
            {
                ratesClassifiedDisplay = ratesClassifiedDisplay.Where(f => f.EditionId == EditionConstants.National);
                if (!ratesClassifiedDisplay.Any())
                    return model;
            }

            //Get order total amount
            model.EstimatedTotalAmount = await GetOrderTotalAmountInfo(ratesClassifiedDisplay, model.MasterTableId, model.BookingNo);
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

            var dateOfferPercentage = classifiedDisplay.DiscountPercent;
            var manualDiscountPercentage = model.ManualDiscountPercentage;
            model.TotalDiscountPercentages = dateOfferPercentage + model.EditionDiscountPercentage + manualDiscountPercentage;

            //Get net amount
            model.NetAmount = GetNetAmount(model);

            model.Commission = 0;
            if (CurrentLoginUser.IsCorrespondentUser)
                model.Commission = (model.NetAmount * CurrentLoginUser.DefaultCommission) / 100;

            //agency commission
            if (!CurrentLoginUser.IsCorrespondentUser && classifiedDisplay.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsById((int)classifiedDisplay.AgencyId);
                if (agency != null && agency.AgencyId > 0)
                    model.Commission = (model.NetAmount * agency.CDCommission) / 100;
            }

            decimal vat = 0;
            if (model.NationalEdition) vat = 15;
            model.VATAmount = ((model.NetAmount - model.Commission) * vat) / 100;

            return model;
        }

        private decimal GetNetAmount(OrderTotalCalculationViewModel model)
        {
            decimal netAmount = 0;
            //let's calculate net amount 
            netAmount = model.EstimatedTotalAmount - ((model.EstimatedTotalAmount * model.TotalDiscountPercentages) / 100);

            return netAmount;
        }


        private async Task<decimal> GetOrderTotalAmountInfo(IEnumerable<RatePrintClassifiedDisplay> ratesClassifiedDisplay, int masterTableId, string bookingNo)
        {
            decimal orderTotalAmount = 0;

            //get stored classified text from session
            var classifiedDisplay = HttpContext.Session.GetObject<ABPrintClassifiedDisplay>(SessionKeyConstants.ABPrintClassifiedDisplay);
            if (classifiedDisplay == null)
            {
                //get classified text from database
                classifiedDisplay = await abPrintClassifiedDisplayService.GetDetailsByIdAndBookingNo(masterTableId, bookingNo);
                if (classifiedDisplay == null)
                    return 0;
            }

            decimal height = classifiedDisplay.AdColumnInch;
            foreach (var rate in ratesClassifiedDisplay)
            {
                //Get classified  display ad rate
                decimal adRate = GetClassifiedDisplayAdRate(classifiedDisplay.AdColumnInch, rate);
                decimal baseAmount = adRate;
                orderTotalAmount = orderTotalAmount + baseAmount;
            }

            return orderTotalAmount;
        }

        private decimal GetClassifiedDisplayAdRate(decimal adColumnInch, RatePrintClassifiedDisplay rateClassifiedDisplay)
        {
            decimal adColumnInchRate = 0;
            switch (adColumnInch)
            {
                case ClassifiedDisplayColumnInchConstants.Inch_1:
                    adColumnInchRate = rateClassifiedDisplay.Rate1ColInch;
                    break;

                case ClassifiedDisplayColumnInchConstants.Inch_1_5:
                    adColumnInchRate = rateClassifiedDisplay.Rate15ColInch;
                    break;

                case ClassifiedDisplayColumnInchConstants.Inch_2:
                    adColumnInchRate = rateClassifiedDisplay.Rate2ColInch;
                    break;

                case ClassifiedDisplayColumnInchConstants.Inch_2_5:
                    adColumnInchRate = rateClassifiedDisplay.Rate25ColInch;
                    break;

                case ClassifiedDisplayColumnInchConstants.Inch_3:
                    adColumnInchRate = rateClassifiedDisplay.Rate3ColInch;
                    break;

                case ClassifiedDisplayColumnInchConstants.Inch_3_5:
                    adColumnInchRate = rateClassifiedDisplay.Rate35ColInch;
                    break;

                case ClassifiedDisplayColumnInchConstants.Inch_4:
                    adColumnInchRate = rateClassifiedDisplay.Rate4ColInch;
                    break;

                default:
                    adColumnInchRate = 0;
                    break;
            }

            return adColumnInchRate;
        }

        private void ClearSessionData()
        {
            HttpContext.Session.Remove(SessionKeyConstants.ABPrintClassifiedDisplay);
            HttpContext.Session.Remove(SessionKeyConstants.RatePrintClassifiedDisplayByEditions);
            HttpContext.Session.Remove(SessionKeyConstants.RatePrintClassifiedDisplay);
            HttpContext.Session.Remove(SessionKeyConstants.ClassifiedDisplayAdHeight);
        }

        #endregion
    }
}
