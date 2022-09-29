using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Service.ABDigitalDisplays;
using pbAd.Service.Advertisers;
using pbAd.Service.Agencies;
using pbAd.Service.Brands;
using pbAd.Service.CacheManagerServices;
using pbAd.Service.Categories;
using pbAd.Service.DefaultDiscounts;
using pbAd.Service.Editions;
using pbAd.Service.PaymentGateways;
using pbAd.Service.PaymentGateways.Models;
using pbAd.Service.RateDigitalDisplays;
using pbAd.Service.SSLPaymentTransactions;
using pbAd.Web.Infrastructure.Helpers;
using pbAd.Web.ViewModels.BookDigitalDisplayAds;
using pbAd.Web.ViewModels.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.Controllers
{
    public class CheckoutDigitalDisplayController : AdminBaseController
    {
        #region Private Methods
        private readonly IEditionService editionService;
        private readonly IABDigitalDisplayService abDigitalDisplayService;
        private readonly IDefaultDiscountService defaultDiscountService;
        private readonly IRateDigitalDisplayService rateDigitalDisplayServiceService;
        private readonly ICategoryService categoryService;
        private readonly IAdvertiserService advertiserService;
        private readonly IBrandService brandService;
        private readonly IAgencyService agencyService;
        private readonly IRateDigitalDisplayService rateDigitalDisplayService;
        private readonly ISSLPaymentTransactionService sslPaymentTransactionService;
      
        private readonly ICacheManagerService cacheManagerService;
        private readonly IConfiguration configuration;
        #endregion

        #region Ctor

        public CheckoutDigitalDisplayController(IEditionService editionService,
                IABDigitalDisplayService abDigitalDisplayService,
                IDefaultDiscountService defaultDiscountService,
                ICategoryService categoryService,
                IAdvertiserService advertiserService,
                IBrandService brandService,
                IAgencyService agencyService,
              
                IConfiguration configuration,
                IRateDigitalDisplayService rateDigitalDisplayService,
                ISSLPaymentTransactionService sslPaymentTransactionService,
                ICacheManagerService cacheManagerService,
                IRateDigitalDisplayService rateDigitalDisplayServiceService
            )
        {
            this.editionService = editionService;
            this.abDigitalDisplayService = abDigitalDisplayService;
            this.rateDigitalDisplayServiceService = rateDigitalDisplayServiceService;
            this.defaultDiscountService = defaultDiscountService;
            this.categoryService = categoryService;
            this.advertiserService = advertiserService;
            this.brandService = brandService;
           
            this.agencyService = agencyService;
            this.configuration = configuration;
            this.rateDigitalDisplayService = rateDigitalDisplayService;
            this.sslPaymentTransactionService = sslPaymentTransactionService;
            this.cacheManagerService = cacheManagerService;
        }

        #endregion

        #region Payment

        [Route("/checkout/digitaldisplay/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(int id, string bookingno)
        {
            var bookingStep = BookingStepConstants.Booked;
            var digitalDisplay = await abDigitalDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);
            if (digitalDisplay == null)
                return Redirect("/bookdigitaldisplayad/booknow");

            var digitalDisplayDetailListing = await abDigitalDisplayService.GetABDigitalDisplayDetailListing(id);

            if (digitalDisplayDetailListing == null)
                return Redirect("/bookdigitaldisplayad/booknow");

            var digitalDisplayDetail = digitalDisplayDetailListing.OrderBy(f => f.PublishDateStart).FirstOrDefault();
            var billDate = digitalDisplayDetail.PublishDateStart;

            var category = await categoryService.GetDetailsById(digitalDisplay.CategoryId);
            if (category == null) category = new Category();

            var brand = await brandService.GetDetailsById(digitalDisplay.BrandId.Value);
            if (brand == null) brand = new Brand();

            var agency = await agencyService.GetDetailsById(digitalDisplay.AgencyId.Value);
            if (agency == null) agency = new Agency();

            var advertiser = await advertiserService.GetDetailsById(digitalDisplay.AdvertiserId);
            if (advertiser == null) advertiser = new Advertiser();

            //get media content listings
            var mediaContentListing = await abDigitalDisplayService.GetDigitalDisplayMediaContentListing(id);

            var model = new CheckoutDigitalDisplayViewModel
            {
                ABDigitalDisplay = digitalDisplay,
                DigitalDisplayDetailListing = digitalDisplayDetailListing,
                MediaContentListing = mediaContentListing,
                ABDigitalDisplayId = id,
                BookingNo = bookingno,
                ManualDiscountPercentage = 0,
                Remarks = digitalDisplay.Remarks,
                AdvertiserName = advertiser?.AdvertiserName,
                AdvertiserMobile = advertiser?.AdvertiserMobileNo,
                BrandName = brand?.BrandName,
                CategoryName = category?.CategoryName,
                AgencyName = agency?.AgencyName,
                CollectorId = agency?.CollectorId,
                AgencyId = agency?.AgencyId,
                BillDate = billDate
            };

            //Get default payment mode & discount
            await GetDefaultPaymentModeAndDiscount(model);

            digitalDisplay.DigitalDisplayMediaContents = null;

            //store private display into session
            HttpContext.Session.SetObject(SessionKeyConstants.ABDigitalDisplay, digitalDisplay);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/digitaldisplay/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(CheckoutDigitalDisplayViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                if (!model.AcceptTermsAndConditions)
                    return Json(new { status = false, message = "Please accept terms and conditions", type = ActionResultTypeConstants.Message });

                //get stored private display from session
                var digitalDisplay = HttpContext.Session.GetObject<ABDigitalDisplay>(SessionKeyConstants.ABDigitalDisplay);

                if (digitalDisplay == null)
                    return Json(new { status = false, message = "Warning! Classified Text Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                if ((digitalDisplay.ABDigitalDisplayId != model.ABDigitalDisplayId)
                    || (digitalDisplay.BookingNo != model.BookingNo))
                    return Json(new { status = false, message = "Warning! Digital Display Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                //re-generate digital display
                var updatePrivateDisplay = await RegenerateABDigitalDisplayInfo(model);

                //let's Add ABDigitalDisplay Detail
                var IsUpdated = await abDigitalDisplayService.CheckoutDigitalDisplay(updatePrivateDisplay);

                if (!IsUpdated)
                    return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/digitaldisplay/{model.ABDigitalDisplayId}/Confirmation/{model.BookingNo}";

                if (CurrentLoginUser.IsCorrespondentUser)
                    returnUrl = $"/checkout/digitaldisplay/{model.ABDigitalDisplayId}/payment-complete/{model.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [AllowAnonymous]
        [Route("/checkout/digitaldisplay/{id}/confirmation/{bookingno}")]
        [Route("/checkout/digitaldisplay/{id}/confirmation/{bookingno}/mode/{paymentmode}")]
        [Route("/checkout/digitaldisplay/{id}/confirmation/{bookingno}/mode/{paymentmode}/adtype/{adtype}")]
        public async Task<IActionResult> Confirmation(int id, string bookingno, int paymentmode, string adtype)
        {
            try
            {
                var updatedBy = CurrentLoginUser.UserId;
                if (paymentmode == CheckoutPaymentTypeConstants.Card)
                {
                    var userInfo= cacheManagerService.GetSSLInUserInfo(bookingno);
                    ViewBag.SSLUserInfo = userInfo;

                    updatedBy = userInfo.UserId;

                    //Populate SSL Payment Transaction
                    SSLPaymentTransaction updateSSLPaymentTransaction = PopulateSSLPaymentTransaction(id, adtype);

                    var updateABDigitalDisplay = new ABDigitalDisplay
                    {
                        ABDigitalDisplayId = id,
                        ApprovalStatus = ApproveStatusConstants.Approved,
                        PaymentStatus = PaymentStatusConstants.Paid,
                        AdStatus = AdStatusConstants.Confirmed_And_Paid,
                        ActualReceived = Convert.ToDecimal(updateSSLPaymentTransaction.Amount),
                        PaymentTrxId = updateSSLPaymentTransaction.Bank_Tran_Id,
                        PaymentModeId = paymentmode,
                        MoneyReceiptNo = updateSSLPaymentTransaction.Bank_Tran_Id,
                        PaymentMobileNumber = PaymentModeConstants.GetText(PaymentModeConstants.SSL),
                        ModifiedBy = updatedBy
                    };
                    //let's update payment mode
                    await abDigitalDisplayService.UpdatePaymentMode(updateABDigitalDisplay);

                    await sslPaymentTransactionService.Update(updateSSLPaymentTransaction);

                    cacheManagerService.RemoveSSLUserCacheInfo(bookingno);
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

        [Route("/checkout/digitaldisplay/{id}/payment-complete/{bookingno}")]
        public async Task<IActionResult> PaymentComplete(int id, string bookingno)
        {
            if (!CurrentLoginUser.IsCorrespondentUser)
                return Redirect("/bookdigitaldisplayad/booknow");

            var bookingStep = BookingStepConstants.Checkout;
            var digitalDisplay = await abDigitalDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

            if (digitalDisplay == null)
                return Redirect("/bookdigitaldisplayad/booknow");

            var netPayable = Convert.ToInt32(Math.Round(digitalDisplay.NetAmount) + Math.Round(digitalDisplay.VATAmount) - Math.Round(digitalDisplay.Commission));

            var model = new PaymentViewModel
            {
                MasterId = digitalDisplay.ABDigitalDisplayId,
                BookingNo = bookingno,
                NetPayable = netPayable
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/digitaldisplay/{id}/payment-complete/{bookingno}")]
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
                var updateDigitalDisplay = PopulatePaymentCompleteInfo(model);

                //let's update ABDigitalDisplay
                var isUpdated = await abDigitalDisplayService.UpdatePaymentInfo(updateDigitalDisplay);

                if (!isUpdated)
                    return Json(new { status = false, message = "Error Occurred. Contact with support team.", type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/digitaldisplay/{model.MasterId}/confirmation/{model.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [HttpGet]
        [Route("/checkout/digitaldisplay/{id}/payment-complete/{bookingno}/card")]
        public async Task<IActionResult> PaymentCompleteWithCard(int id, string bookingno)
        {
            try
            {
                if (!CurrentLoginUser.IsCorrespondentUser)
                    return Redirect("/bookdigitaldisplayad/booknow");

                var bookingStep = BookingStepConstants.Checkout;
                var digitalDisplay = await abDigitalDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

                if (digitalDisplay == null)
                    return Redirect("/bookdigitaldisplayad/booknow");

                var netPayable = Convert.ToInt32(Math.Round(digitalDisplay.NetAmount) + Math.Round(digitalDisplay.VATAmount) - Math.Round(digitalDisplay.Commission));

                var trxtId = $"DD-{bookingno}";

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
                cacheManagerService.SetSSLKeysValue(bookingno,CurrentLoginUser.UserId);

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
            //get stored digital display from session
            var digitalDisplay = HttpContext.Session.GetObject<ABDigitalDisplay>(SessionKeyConstants.ABDigitalDisplay);
            if (digitalDisplay == null)
            {
                //get private display from database
                digitalDisplay = await abDigitalDisplayService.GetDetails(model.MasterTableId, model.BookingNo, CurrentLoginUser.UserId);
                if (digitalDisplay == null)
                    return Json(new { status = false, message = "Ad configuration not found. Please contact with support", type = ActionResultTypeConstants.Message });
            }

            //Get order calculations
            model = await GetOrderCalculations(model);

            return Json(new
            {
                status = true,
                EstimatedTotalAmount = model.EstimatedTotalAmount,
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

            //Get Default Discount
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
                AdType = AdTypeConstants.DigitalDisplay,
                AdMasterId = adMasterId,
                Amount = netPayable,
                GatwayNote = $@"{AdTypeConstants.DigitalDisplay} ad with {bookingno}. Total Amount {netPayable}",
                FailedReason = null,
                RedirectGatewayURL = sslRedirectUrl,
                PaymentTransactionStatus = PaymentTransactionStatusConstants.InProgress,
                CreatedBy = CurrentLoginUser.UserId,
            };
        }

        private async Task<ABDigitalDisplay> RegenerateABDigitalDisplayInfo(CheckoutDigitalDisplayViewModel model)
        {
            var defaultDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);

            //get stored digital display from session
            var digitalDisplay = HttpContext.Session.GetObject<ABDigitalDisplay>(SessionKeyConstants.ABDigitalDisplay);
            if (digitalDisplay == null)
            {
                //get digital display from database
                digitalDisplay = await abDigitalDisplayService.GetDetails(model.MasterTableId, model.BookingNo, CurrentLoginUser.UserId);

                if (digitalDisplay != null) //set session data for digital display                 
                    HttpContext.Session.SetObject(SessionKeyConstants.ABDigitalDisplay, digitalDisplay);
            }

            var orderTotalCalculationModel = new OrderTotalCalculationViewModel
            {
                MasterTableId = digitalDisplay.ABDigitalDisplayId,
                BookingNo = digitalDisplay.BookingNo,
                ManualDiscountPercentage = model.ManualDiscountPercentage,
                AgencyId = digitalDisplay.AgencyId,
                IsFixed = model.IsFixed,
                FixedAmount = model.FixedAmount
            };

            //Get order calculations
            var orderTotalCalculation = await GetOrderCalculations(orderTotalCalculationModel);

            var newPrivateDisplay = new ABDigitalDisplay
            {
                ABDigitalDisplayId = digitalDisplay.ABDigitalDisplayId,
                BookingNo = digitalDisplay.BookingNo,
                BookingStep = BookingStepConstants.Checkout,
                AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                GrossTotal = Math.Round(orderTotalCalculation.EstimatedTotalAmount, 0, MidpointRounding.AwayFromZero),
                DiscountPercent = model.ManualDiscountPercentage,
                PaymentModeId = model.PaymentModeId,
                PaymentStatus = PaymentStatusConstants.Not_Paid,
                IsFixed = model.IsFixed,
                BillNo = await GenerateBillNo(model.PaymentModeId),
                BillDate = Convert.ToDateTime(model.BillDate),
                ModifiedBy = CurrentLoginUser.UserId
            };

            newPrivateDisplay.DiscountAmount = Math.Round((newPrivateDisplay.GrossTotal * orderTotalCalculation.TotalDiscountPercentages) / 100, 0, MidpointRounding.AwayFromZero);
            newPrivateDisplay.NetAmount = Math.Round(orderTotalCalculation.NetAmount, 0, MidpointRounding.AwayFromZero);

            newPrivateDisplay.Commission = Math.Round(orderTotalCalculation.Commission, 0, MidpointRounding.AwayFromZero);
            newPrivateDisplay.VATAmount = Math.Round(orderTotalCalculation.VATAmount, 0, MidpointRounding.AwayFromZero);

            //approval status
            newPrivateDisplay.ApprovalStatus = (model.ManualDiscountPercentage <= defaultDiscountPercentage)
                                                                ? ApproveStatusConstants.Approved : ApproveStatusConstants.Pending_Approval_Layer1;

            if (model.IsFixed) newPrivateDisplay.ApprovalStatus = ApproveStatusConstants.Pending_Approval_Layer1;

            if (model.PaymentModeId == PaymentModeConstants.Credit && model.AgencyId > 0)
                newPrivateDisplay.CollectorId = model.CollectorId;

            return newPrivateDisplay;
        }

        private async Task<OrderTotalCalculationViewModel> GetOrderCalculations(OrderTotalCalculationViewModel model)
        {
            //Get order total amount
            var estimatedTotalAmount = await GetDigitalDisplayOrderTotalAmount(model.MasterTableId);
            model.EstimatedTotalAmount = estimatedTotalAmount;

            model.TotalDiscountPercentages = model.ManualDiscountPercentage;

            //Get net amount
            model.NetAmount = GetNetAmount(model);

            model.Commission = 0;
            if (CurrentLoginUser.IsCorrespondentUser)
                model.Commission = (model.NetAmount * CurrentLoginUser.DefaultCommission) / 100;

            //agency commission
            if (!CurrentLoginUser.IsCorrespondentUser && model.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsById((int)model.AgencyId);
                if (agency != null && agency.AgencyId > 0)
                    model.Commission = (model.NetAmount * agency.DDCommission) / 100;
            }

            decimal vat = 0;
            if (model.NationalEdition) vat = 15;
            model.VATAmount = ((model.NetAmount - model.Commission) * vat) / 100;

            if (model.IsFixed)
            {
                model.ManualDiscountPercentage = 0;
                model.TotalDiscountPercentages = 0;
                model.Commission = 0;

                if (model.NationalEdition) vat = 15;
                model.VATAmount = (model.FixedAmount * vat / 100);

                model.NetAmount = model.FixedAmount - model.VATAmount;
            }

            return model;
        }

        private decimal GetNetAmount(OrderTotalCalculationViewModel model)
        {
            decimal netAmount = 0;
            //let's calculate net amount 
            netAmount = model.EstimatedTotalAmount - ((model.EstimatedTotalAmount * model.TotalDiscountPercentages) / 100);

            return netAmount;
        }

        private async Task<decimal> GetDigitalDisplayOrderTotalAmount(int digitalDisplayId)
        {
            decimal estimatedCosting = 0;

            var digitalDisplayDetailListing = await abDigitalDisplayService.GetABDigitalDisplayDetailListing(digitalDisplayId);

            if (digitalDisplayDetailListing.Any())
            {
                estimatedCosting = digitalDisplayDetailListing.Sum(f => f.PerUnitRate);
            }

            return estimatedCosting;
        }

        private async Task<DigitalDisplayEstimatedCostingViewModel> FetchEstimatedTotalAmount(DigitalDisplayEstimatedCostingViewModel model)
        {
            var ratesDigitalDisplay = await rateDigitalDisplayService.GetDigitalDisplayRates();

            decimal estimatedCosting = 0;

            if (ratesDigitalDisplay.Any())
            {
                foreach (var item in model.DigitalDisplayDetailList)
                {
                    var rateDigitalDisplay = ratesDigitalDisplay.FirstOrDefault(f => f.DigitalAdUnitTypeId == item.DigitalAdUnitTypeId
                                        && f.DigitalPageId == item.DigitalPageId
                                        && f.DigitalPagePositionId == item.DigitalPagePositionId);

                    if (rateDigitalDisplay == null || rateDigitalDisplay.AutoId <= 0) continue;

                    estimatedCosting = estimatedCosting + rateDigitalDisplay.PerUnitRate * item.AdQty;
                }
            }

            model.EstimatedCosting = Math.Round(estimatedCosting, 0, MidpointRounding.AwayFromZero);

            return model;
        }


        private async Task<string> GenerateBillNo(int paymentModeId)
        {
            var billNo = string.Empty;
            try
            {
                int totalCount = 0; string billNumber = "";
                totalCount = await abDigitalDisplayService.GetTotalCountByPaymentMode(paymentModeId);
                billNumber = GetBillNumber(totalCount);

                if (paymentModeId == PaymentModeConstants.Cash || paymentModeId == PaymentModeConstants.SSL)
                    billNo = $@"AJP/DD/RPT/{billNumber}";
                else
                    billNo = $@"AJP/DD/CR/{billNumber}";
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

        private PaymentGatewayRequest PopulatePaymentGatewayRequest(int id, string bookingno, decimal totalPrice, string labReceiptNo)
        {
            var adType = AdTypeConstants.DigitalDisplay;
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var products = AdTypeConstants.DigitalDisplay;
            var successUrl = $"{baseUrl}/checkout/digitaldisplay/{id}/confirmation/{bookingno}/mode/{CheckoutPaymentTypeConstants.Card}/adtype/{adType}";

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

        private ABDigitalDisplay PopulatePaymentCompleteInfo(PaymentViewModel model)
        {
            var updateDigitalDisplay = new ABDigitalDisplay();
            if (model.PaymentType == CheckoutPaymentTypeConstants.Direct)
            {
                updateDigitalDisplay = new ABDigitalDisplay
                {
                    ABDigitalDisplayId = model.MasterId,
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
                updateDigitalDisplay = new ABDigitalDisplay
                {
                    ABDigitalDisplayId = model.MasterId,
                    PaymentType = model.PaymentType,
                    PaymentMethod = PaymentMethodConstants.Check_Or_Payorder,
                    PaymentMobileNumber = model.CheckInfo,
                    PaymentTrxId = model.BankInfo,
                    PaymentPaidAmount = model.CheckOrPayorderAmount,
                    BookingStep = BookingStepConstants.Completed,
                    ModifiedBy = CurrentLoginUser.UserId
                };
            }

            return updateDigitalDisplay;
        }

        private void ClearSessionData()
        {
            HttpContext.Session.Remove(SessionKeyConstants.ABDigitalDisplay);
            HttpContext.Session.Remove(SessionKeyConstants.RateABDigitalDisplay);
        }

        private async Task GetDefaultPaymentModeAndDiscount(CheckoutDigitalDisplayViewModel model)
        {
            if (CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User)
            {
                model.PaymentModeId = PaymentModeConstants.Cash;
                var manualDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);
                model.ManualDiscountPercentage = (int)manualDiscountPercentage;
            }
            else if (CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
            {
                model.PaymentModeId = PaymentModeConstants.SSL;
                var manualDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);
                model.ManualDiscountPercentage = (int)manualDiscountPercentage;
            }
        }

        private async Task<decimal> GetDefaultDiscount(int paymentModeId)
        {
            decimal manualDiscountPercentage = 0;
            //get default discount 
            var defaultDiscount = await defaultDiscountService
                .GetDetailsByAdTypeAndPaymentMode(ClassifiedTypeConstants.DigitalDisplay, paymentModeId);

            if (defaultDiscount != null)
                manualDiscountPercentage = defaultDiscount.DiscountRate;

            return manualDiscountPercentage;
        }

        #endregion
    }
}
