
#region Usings

using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Service.ABPrintPrivateDisplays;
using pbAd.Service.DefaultDiscounts;
using pbAd.Service.EditionPages;
using pbAd.Service.Editions;
using pbAd.Service.OfferEditions;
using pbAd.Service.RatePrintPrivateDisplays;
using pbAd.Service.RatePrintSpotAds;
using pbAd.Web.Infrastructure.Helpers;
using pbAd.Web.ViewModels.Checkout;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using pbAd.Service.Agencies;
using pbAd.Service.RatePrintEarPanelAds;
using pbAd.Service.EditionDistricts;
using pbAd.Core.Filters;
using pbAd.Service.Categories;
using pbAd.Service.Advertisers;
using pbAd.Service.Brands;
using pbAd.Service.PaymentGateways;
using pbAd.Service.PaymentGateways.Models;
using Microsoft.AspNetCore.Authorization;
using pbAd.Service.SSLPaymentTransactions;
using Microsoft.Extensions.Configuration;
using pbAd.Service.CacheManagerServices;
using System.Security.Claims;

#endregion

namespace pbAd.Web.Controllers
{
    public class CheckoutPrivateDisplayController : AdminBaseController
    {
        #region Private Methods
        private readonly IEditionService editionService;
        private readonly IABPrintPrivateDisplayService abPrintPrivateDisplayService;
        private readonly IRatePrintPrivateDisplayService ratePrintPrivateDisplayService;
        private readonly IOfferEditionService offerEditionService;
        private readonly IRatePrintSpotAdService ratePrintSpotAdService;
        private readonly IEditionPageService editionPageService;
        private readonly IDefaultDiscountService defaultDiscountService;
        private readonly IAgencyService agencyService;
        private readonly IRatePrintEarPanelAdService ratePrintEarPanelAdService;
        private readonly IEditionDistrictService editionDistrictService;
        private readonly ICategoryService categoryService;
        private readonly IAdvertiserService advertiserService;
        private readonly IBrandService brandService;
        private readonly ISSLPaymentTransactionService sslPaymentTransactionService;
        
        private readonly IConfiguration configuration;
        private readonly ICacheManagerService cacheManagerService;
        #endregion

        #region Ctor

        public CheckoutPrivateDisplayController(IEditionService editionService,
                IABPrintPrivateDisplayService abPrintPrivateDisplayService,
                IDefaultDiscountService defaultDiscountService,
                IOfferEditionService offerEditionService,
                IRatePrintSpotAdService ratePrintSpotAdService,
                IEditionPageService editionPageService,
                IRatePrintPrivateDisplayService ratePrintPrivateDisplayService,
                IAgencyService agencyService,
                IRatePrintEarPanelAdService ratePrintEarPanelAdService,
                ICategoryService categoryService,
                IAdvertiserService advertiserService,
                IBrandService brandService,

                ICacheManagerService cacheManagerService,
                IEditionDistrictService editionDistrictService,
                
                IConfiguration configuration,               
                ISSLPaymentTransactionService sslPaymentTransactionService
            )
        {
            this.editionService = editionService;
            this.abPrintPrivateDisplayService = abPrintPrivateDisplayService;
            this.ratePrintPrivateDisplayService = ratePrintPrivateDisplayService;
            this.defaultDiscountService = defaultDiscountService;
            this.offerEditionService = offerEditionService;
            this.ratePrintSpotAdService = ratePrintSpotAdService;
            this.editionPageService = editionPageService;
            this.agencyService = agencyService;
            this.ratePrintEarPanelAdService = ratePrintEarPanelAdService;
            this.editionDistrictService = editionDistrictService;
            this.categoryService = categoryService;
            this.advertiserService = advertiserService;
            this.brandService = brandService;
           
            this.configuration = configuration;
            this.cacheManagerService = cacheManagerService;
            this.sslPaymentTransactionService = sslPaymentTransactionService;
        }

        #endregion

        #region Payment

        [Route("/checkout/privatedisplay/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(int id, string bookingno)
        {
            //get private display
            var bookingStep = BookingStepConstants.Booked;
            var privateDisplay = await abPrintPrivateDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);
            if (privateDisplay == null)
                return Redirect("/bookprivatedisplayad/booknow");

            //get private display detail listings
            var privateDisplayDetailListing = await abPrintPrivateDisplayService.GetABPrintPrivateDisplayDetailListing(id);

            if (!privateDisplayDetailListing.Any())
                return Redirect("/bookprivatedisplayad/booknow");

            var privateDisplayDetail = privateDisplayDetailListing.FirstOrDefault();
            var editionPage = await editionPageService.GetDetailsById(privateDisplayDetail.EditionPageId);

            if (editionPage == null)
                return Redirect("/bookprivatedisplayad/booknow");

            //get media content listings
            var displayMediaContentListing = await abPrintPrivateDisplayService.GetPrivateDisplayMediaContentListing(id);

            var filter = new EditionSearchFilter { IsColor = privateDisplay.IsColor, EditionPageNo = editionPage.EditionPageNo, PrivateAdType = privateDisplay.PrivateAdType };

            //get editions
            var edtions = await ratePrintPrivateDisplayService.GetAllEditions(filter);

            var selectedEditionId = privateDisplayDetail.EditionId;

            //get publish date as base offer dates
            var basedOfferDates = privateDisplayDetailListing.OrderBy(o=>o.PublishDate).Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();

            var basedOfferDatesInString = String.Join(", ", basedOfferDates);

            //get default editions
            var defaultEdtion = edtions.FirstOrDefault(f => f.EditionId == selectedEditionId);

            if (defaultEdtion == null)
                defaultEdtion = new Edition();

            //editions except default edition
            var filteredEditions = edtions.Where(f => f.EditionId != EditionConstants.National).ToList();

            var editionDistrictList = await editionDistrictService.GetAllEditionDistricts();
            var editionDistrict = editionDistrictList.FirstOrDefault(f => f.EditionId == defaultEdtion.EditionId);

            var category = await categoryService.GetDetailsById(privateDisplay.CategoryId);
            if (category == null) category = new Category();

            var brand = await brandService.GetDetailsById(privateDisplay.BrandId.Value);
            if (brand == null) brand = new Brand();

            var agency = await agencyService.GetDetailsById(privateDisplay.AgencyId.Value);
            if (agency == null) agency = new Agency();

            var advertiser = await advertiserService.GetDetailsById(privateDisplay.AdvertiserId);
            if (advertiser == null) advertiser = new Advertiser();

            var model = new CheckoutPrivateDisplayViewModel
            {
                ABPrintPrivateDisplay = privateDisplay,
                PrivateAdType = privateDisplay.PrivateAdType,
                EditionPageId = privateDisplayDetail.EditionPageId,
                EditionPageName = $"{editionPage.EditionPageName} (Page - {editionPage.EditionPageNo})",
                EditionPageNo = editionPage.EditionPageNo,
                Editions = filteredEditions,
                BasedOfferDatesInString = basedOfferDatesInString,
                ABPrintPrivateDisplayId = id,
                BookingNo = bookingno,
                DefaultEditionName = defaultEdtion.EditionName,
                DefaultEditionId = defaultEdtion.EditionId,
                ManualDiscountPercentage = 0,
                DisplayMediaContentListing = displayMediaContentListing,
                Remarks = privateDisplay.Remarks,
                EditionDistrictList = editionDistrictList,
                AdvertiserName = advertiser?.AdvertiserName,
                AdvertiserMobile = advertiser?.AdvertiserMobileNo,
                BrandName = brand?.BrandName,
                CategoryName = category?.CategoryName,
                AgencyName = agency?.AgencyName,
                CollectorId = agency?.CollectorId,
                AgencyId = agency?.AgencyId,
                BillDate = basedOfferDates.FirstOrDefault()
            };

            //Get default payment mode & discount
            await GetDefaultPaymentModeAndDiscount(model);

            privateDisplay.PrivateDisplayMediaContents = null;
            //store private display into session
            HttpContext.Session.SetObject(SessionKeyConstants.ABPrintPrivateDisplay, privateDisplay);
            HttpContext.Session.SetInt(SessionKeyConstants.PrivateDisplayTotalPublishDates, basedOfferDates.Count());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/privatedisplay/{id}/payment/{bookingno}")]
        public async Task<IActionResult> Payment(CheckoutPrivateDisplayViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                if (!model.AcceptTermsAndConditions)
                    return Json(new { status = false, message = "Please accept terms and conditions", type = ActionResultTypeConstants.Message });

                //get stored private display from session
                var privateDisplay = HttpContext.Session.GetObject<ABPrintPrivateDisplay>(SessionKeyConstants.ABPrintPrivateDisplay);

                if (privateDisplay == null)
                    return Json(new { status = false, message = "Warning! Classified Text Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                if ((privateDisplay.ABPrintPrivateDisplayId != model.ABPrintPrivateDisplayId)
                    || (privateDisplay.BookingNo != model.BookingNo))
                    return Json(new { status = false, message = "Warning! Private Display Configuration not found. Please reload this page and try again!", type = ActionResultTypeConstants.Message });

                if (model.NationalEdition)
                {
                    var defaultEditionId = EditionConstants.National;
                    model.EditionIds = new List<int>();
                    model.EditionIds.Add(defaultEditionId);
                }

                //re-generate ab print private display
                var updatePrivateDisplay = await RegenerateABPrintPrivateDisplayInfo(model);

                //Populate ab print classified display detail
                var abPrintPrivateDisplayDetailListing = PopulateABPrintPrivateDisplayDetail(model);

                var checkProcessRequest = new CheckoutPrivateDisplayProcessModel
                {
                    ABPrintPrivateDisplayId = model.ABPrintPrivateDisplayId,
                    ABPrintPrivateDisplay = updatePrivateDisplay,
                    ABPrintPrivateDisplayDetailListing = abPrintPrivateDisplayDetailListing
                };

                //let's Add ABPrintPrivateDisplay Detail
                var response = await abPrintPrivateDisplayService.CheckoutPrivateDisplayDetail(checkProcessRequest);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/privatedisplay/{model.ABPrintPrivateDisplayId}/Confirmation/{model.BookingNo}";

                if (CurrentLoginUser.IsCorrespondentUser && model.PaymentModeId==PaymentModeConstants.SSL)
                    returnUrl = $"/checkout/privatedisplay/{model.ABPrintPrivateDisplayId}/payment-complete/{model.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [AllowAnonymous]
        [Route("/checkout/privatedisplay/{id}/confirmation/{bookingno}")]
        [Route("/checkout/privatedisplay/{id}/confirmation/{bookingno}/mode/{paymentmode}")]
        [Route("/checkout/privatedisplay/{id}/confirmation/{bookingno}/mode/{paymentmode}/adtype/{adtype}")]
        public async Task<IActionResult> Confirmation(int id, string bookingno,int paymentmode, string adtype)
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

                    var updateABPrintPrivateDisplay = new ABPrintPrivateDisplay
                    {
                        ABPrintPrivateDisplayId = id,
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
                    await abPrintPrivateDisplayService.UpdatePaymentMode(updateABPrintPrivateDisplay);

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

        [Route("/checkout/privatedisplay/{id}/payment-complete/{bookingno}")]
        public async Task<IActionResult> PaymentComplete(int id, string bookingno)
        {
            if (!CurrentLoginUser.IsCorrespondentUser)
                return Redirect("/bookprivatedisplayad/booknow");

            var bookingStep = BookingStepConstants.Checkout;
            var privateDisplay = await abPrintPrivateDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

            if (privateDisplay == null)
                return Redirect("/bookprivatedisplayad/booknow");

            var netPayable = Convert.ToInt32(Math.Round(privateDisplay.NetAmount) + Math.Round(privateDisplay.VATAmount) - Math.Round(privateDisplay.Commission));

            var model = new PaymentViewModel
            {
                MasterId = privateDisplay.ABPrintPrivateDisplayId,
                BookingNo = bookingno,
                NetPayable= netPayable,
                PaymentModeId = privateDisplay.PaymentModeId??0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/checkout/privatedisplay/{id}/payment-complete/{bookingno}")]
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
                var updatePrivateDisplay = PopulatePaymentCompleteInfo(model);

                //let's update ABPrintPrivateDisplay
                var isUpdated = await abPrintPrivateDisplayService.UpdatePaymentInfo(updatePrivateDisplay);

                if (!isUpdated)
                    return Json(new { status = false, message = "Error Occurred. Contact with support team.", type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/privatedisplay/{model.MasterId}/confirmation/{model.BookingNo}/mode/{model.PaymentType}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        [HttpGet]
        [Route("/checkout/privatedisplay/{id}/payment-complete/{bookingno}/card")]
        public async Task<IActionResult> PaymentCompleteWithCard(int id, string bookingno)
        {
            try
            {
                if (!CurrentLoginUser.IsCorrespondentUser)
                    return Redirect("/bookprivatedisplayad/booknow");

                var bookingStep = BookingStepConstants.Checkout;
                var privateDisplay = await abPrintPrivateDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);

                if (privateDisplay == null)
                    return Redirect("/bookprivatedisplayad/booknow");

                var netPayable = Convert.ToInt32(Math.Round(privateDisplay.NetAmount) + Math.Round(privateDisplay.VATAmount) - Math.Round(privateDisplay.Commission));

                var trxtId = $"PD-{bookingno}";
               
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
            //get stored private display from session
            var privateDisplay = HttpContext.Session.GetObject<ABPrintPrivateDisplay>(SessionKeyConstants.ABPrintPrivateDisplay);
            if (privateDisplay == null)
            {
                //get private display from database
                privateDisplay = await abPrintPrivateDisplayService.GetDetails(model.MasterTableId, model.BookingNo, CurrentLoginUser.UserId);
                if (privateDisplay == null)
                    return Json(new { status = false, message = "Ad configuration not found. Please contact with support", type = ActionResultTypeConstants.Message });
            }

            model.PrivateAdType = privateDisplay.PrivateAdType;

            //Get order calculations
            model = await GetOrderCalculations(model);

            return Json(new
            {
                status = true,
                EstimatedTotalAmount = model.EstimatedTotalAmount,
                EditionDiscountPercentage = model.EditionDiscountPercentage,
                DateOfferPercentage = model.DateOfferPercentage,
                NetAmount = model.NetAmount,
                Commission = model.Commission,
                VATAmount = model.VATAmount,
                AdRate = model.AdRate,
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
                AdType = AdTypeConstants.PrivateDisplay,
                AdMasterId = adMasterId,
                Amount = netPayable,
                GatwayNote = $@"{AdTypeConstants.PrivateDisplay} ad with {bookingno}. Total Amount {netPayable}",
                FailedReason = null,
                RedirectGatewayURL = sslRedirectUrl,
                PaymentTransactionStatus = PaymentTransactionStatusConstants.InProgress,
                CreatedBy = CurrentLoginUser.UserId,
            };
        }

        private PaymentGatewayRequest PopulatePaymentGatewayRequest(int id, string bookingno , decimal totalPrice, string labReceiptNo)
        {
            var adType = AdTypeConstants.PrivateDisplay;
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var products = AdTypeConstants.PrivateDisplay;
            var successUrl = $"{baseUrl}/checkout/privatedisplay/{id}/confirmation/{bookingno}/mode/{CheckoutPaymentTypeConstants.Card}/adtype/{adType}";

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

        private ABPrintPrivateDisplay PopulatePaymentCompleteInfo(PaymentViewModel model)
        {
            var updateClassifiedText = new ABPrintPrivateDisplay();
            if (model.PaymentType == CheckoutPaymentTypeConstants.Direct)
            {
                updateClassifiedText = new ABPrintPrivateDisplay
                {
                    ABPrintPrivateDisplayId = model.MasterId,
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
                updateClassifiedText = new ABPrintPrivateDisplay
                {
                    ABPrintPrivateDisplayId = model.MasterId,
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
                totalCount = await abPrintPrivateDisplayService.GetTotalCountByPaymentMode(paymentModeId);
                billNumber = GetBillNumber(totalCount);

                if (paymentModeId == PaymentModeConstants.Cash || paymentModeId == PaymentModeConstants.SSL)                                 
                    billNo = $@"PB/PD/RPT/{billNumber}";                
                else                                  
                    billNo = $@"PB/PD/CR/{billNumber}";                
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

        private void ClearSessionData()
        {
            HttpContext.Session.Remove(SessionKeyConstants.ABPrintPrivateDisplay);
            HttpContext.Session.Remove(SessionKeyConstants.RatePrintPrivateDisplay);
            HttpContext.Session.Remove(SessionKeyConstants.RatePrintPrivateDisplayByEditions);
        }

        private async Task<ABPrintPrivateDisplay> RegenerateABPrintPrivateDisplayInfo(CheckoutPrivateDisplayViewModel model)
        {
            var defaultDiscountPercentage = await GetDefaultDiscount(model.PaymentModeId);

            //get stored private display from session
            var privateDisplay = HttpContext.Session.GetObject<ABPrintPrivateDisplay>(SessionKeyConstants.ABPrintPrivateDisplay);

            var orderTotalCalculationModel = new OrderTotalCalculationViewModel
            {
                EditionIds = model.EditionIds,
                MasterTableId = privateDisplay.ABPrintPrivateDisplayId,
                BookingNo = privateDisplay.BookingNo,
                ManualDiscountPercentage = model.ManualDiscountPercentage,
                PrivateAdType = privateDisplay.PrivateAdType,
                NationalEdition = model.NationalEdition,
                EditionPageId = model.EditionPageId,
                IsFixed=model.IsFixed,
                FixedAmount = model.FixedAmount,
                EditionPageNo=model.EditionPageNo
            };

            //Get order calculations
            var orderTotalCalculation = await GetOrderCalculations(orderTotalCalculationModel);

            var newPrivateDisplay = new ABPrintPrivateDisplay
            {
                ABPrintPrivateDisplayId = privateDisplay.ABPrintPrivateDisplayId,
                BookingNo = privateDisplay.BookingNo,
                BookingStep = BookingStepConstants.Checkout,
                AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                GrossTotal = Math.Round(orderTotalCalculation.EstimatedTotalAmount, 0, MidpointRounding.AwayFromZero),
                DiscountPercent = model.ManualDiscountPercentage,
                OfferEditionId = orderTotalCalculation.OfferEditionId,
                PaymentModeId = model.PaymentModeId,
                PaymentStatus = PaymentStatusConstants.Not_Paid,
                IsFixed = model.IsFixed,
                NationalEdition = model.NationalEdition,
                BillNo = await GenerateBillNo(model.PaymentModeId),
                BillDate = Convert.ToDateTime(model.BillDate),
                OfferDateId = orderTotalCalculation.OfferDateId,
                ModifiedBy = CurrentLoginUser.UserId
            };

            newPrivateDisplay.DiscountAmount = Math.Round((newPrivateDisplay.GrossTotal * orderTotalCalculation.TotalDiscountPercentages) / 100, 0, MidpointRounding.AwayFromZero);
            newPrivateDisplay.NetAmount = Math.Round(orderTotalCalculation.NetAmount, 0, MidpointRounding.AwayFromZero);

            newPrivateDisplay.Commission = Math.Round(orderTotalCalculation.Commission, 0, MidpointRounding.AwayFromZero);
            newPrivateDisplay.VATAmount = Math.Round(orderTotalCalculation.VATAmount, 0, MidpointRounding.AwayFromZero);

            //approval status
            newPrivateDisplay.ApprovalStatus = (model.ManualDiscountPercentage <= defaultDiscountPercentage)
                                                                ?  ApproveStatusConstants.Approved : ApproveStatusConstants.Pending_Approval_Layer1;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            int userGroup = Convert.ToInt32(claimsIdentity.Claims.FirstOrDefault(f => f.Type == "GroupId").Value);
            if (model.IsFixed && userGroup!=1) 
                newPrivateDisplay.ApprovalStatus = ApproveStatusConstants.Pending_Approval_Layer1;

            if (model.PaymentModeId == PaymentModeConstants.Credit && model.AgencyId > 0)
                newPrivateDisplay.CollectorId = model.CollectorId;

            if (model.PrivateAdType == PrivateAdTypesConstants.Inhouse)
            {
                newPrivateDisplay.OfferDateId = 0;
                newPrivateDisplay.OfferEditionId = 0;
                newPrivateDisplay.GrossTotal = 0;
                newPrivateDisplay.DiscountPercent = 0;
                newPrivateDisplay.DiscountAmount = 0;
                newPrivateDisplay.NetAmount = 0;
                newPrivateDisplay.Commission = 0;
                newPrivateDisplay.VATAmount = 0;
            }

            return newPrivateDisplay;
        }

        private async Task<OrderTotalCalculationViewModel> GetOrderCalculations(OrderTotalCalculationViewModel model)
        {
            //get stored private display from session
            var abPrintPrivateDisplay = HttpContext.Session.GetObject<ABPrintPrivateDisplay>(SessionKeyConstants.ABPrintPrivateDisplay);
            if (abPrintPrivateDisplay == null)
            {
                //get private display from database
                abPrintPrivateDisplay = await abPrintPrivateDisplayService.GetDetails(model.MasterTableId, model.BookingNo, CurrentLoginUser.UserId);
                if (abPrintPrivateDisplay == null)
                    return model;

                abPrintPrivateDisplay.PrivateDisplayMediaContents = null;
                //set session data for private display
                HttpContext.Session.SetObject(SessionKeyConstants.ABPrintPrivateDisplay, abPrintPrivateDisplay);
            }

            //get total publish dates 
            int totalPublishDates = HttpContext.Session.GetInt(SessionKeyConstants.ClassifiedTextTotalPublishDates);
            if (totalPublishDates <= 0)
            {
                var privateDisplayDetailListing = await abPrintPrivateDisplayService.GetABPrintPrivateDisplayDetailListing(model.MasterTableId);

                if (!privateDisplayDetailListing.Any()) totalPublishDates = 0;

                if (privateDisplayDetailListing.Any())
                {
                    var basedOfferDates = privateDisplayDetailListing.Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();
                    totalPublishDates = basedOfferDates.Count();
                }
            }

            //Get order total amount
            var privateDisplayOrder = await GetPrivateDisplayOrderTotalAmount(abPrintPrivateDisplay, model);
            model.EstimatedTotalAmount = privateDisplayOrder.OrderTotalAmount * Convert.ToDecimal(totalPublishDates);
            model.AdRate = privateDisplayOrder.AdRate;

            //if national edition not found then fetch edition offer
            if (!model.NationalEdition)
            {
                if (model.EditionIds == null)
                    model.EditionIds = new List<int>();

                if (!model.ExceptEditionPageNoFor_Rajshahi_Rangpur.Any(f => f == model.EditionPageNo) && model.EditionIds.Any(EditionId => EditionId == EditionConstants.Rajshahi)
                    && model.EditionIds.Any(EditionId => EditionId == EditionConstants.Rangpur))
                {
                    model.EditionIds = model.EditionIds.Where(EditionId => EditionId != EditionConstants.Rangpur).ToList();
                }

                var noofOfferEditions = model.EditionIds.Count - 1;

                var editionOffer = await offerEditionService.GetByNoofEdition(noofOfferEditions);
                if (editionOffer != null && editionOffer.EditionDateId > 0)
                {
                    model.OfferEditionId = editionOffer.EditionDateId;
                    model.EditionDiscountPercentage = editionOffer.DiscountPercentage;
                }
            }

            var dateOfferPercentage = abPrintPrivateDisplay.DiscountPercent;
            model.DateOfferPercentage = (int)dateOfferPercentage;
            model.OfferDateId = abPrintPrivateDisplay.OfferDateId;

            //var manualDiscountPercentage = model.ManualDiscountPercentage;
            model.TotalDiscountPercentages = dateOfferPercentage + model.EditionDiscountPercentage + model.ManualDiscountPercentage;

            //Get net amount
            model.NetAmount = GetNetAmount(model);

            model.Commission = 0;
            if (CurrentLoginUser.IsCorrespondentUser)
                model.Commission = (model.NetAmount * CurrentLoginUser.DefaultCommission) / 100;

            //agency commission
            if (!CurrentLoginUser.IsCorrespondentUser && abPrintPrivateDisplay.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsById((int)abPrintPrivateDisplay.AgencyId);
                if (agency != null && agency.AgencyId > 0)
                    model.Commission = (model.NetAmount * agency.PDCommission) / 100;
            }

            decimal vat = 0;
            if (model.NationalEdition) vat = 15;
            model.VATAmount = ((model.NetAmount - model.Commission) * vat) / 100;

            if (model.IsFixed)
            {
                model.DateOfferPercentage = 0;
                model.OfferDateId = 0;
                model.EditionDiscountPercentage = 0;                
                model.OfferEditionId = 0;

                model.ManualDiscountPercentage = 0;

                model.TotalDiscountPercentages = 0;

                model.TotalDiscountPercentages = 0;
                model.Commission = 0;

                if (model.NationalEdition) vat = 15;
                model.VATAmount = (model.FixedAmount * vat / 100);

                model.NetAmount = model.FixedAmount- model.VATAmount;   
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

        private async Task<OrderTotalCalculationViewModel> GetPrivateDisplayOrderTotalAmount(ABPrintPrivateDisplay privateDisplay, OrderTotalCalculationViewModel model)
        {
            decimal estimatedCosting = 0;

            //get default rates
            if (model.PrivateAdType == PrivateAdTypesConstants.Private) //for private
            {
                //get rate for private               
                var ratesPrintPrivateDisplay = await ratePrintPrivateDisplayService.GetPrivateDisplayRatesByEditionIdsAndEditionPage(model.EditionIds, model.EditionPageId);
                if (ratesPrintPrivateDisplay == null || !ratesPrintPrivateDisplay.Any()) return model;

                //get rates by default eidtion or national edition exist in editions list            
                if (model.NationalEdition)
                {
                    ratesPrintPrivateDisplay = ratesPrintPrivateDisplay.Where(f => f.EditionId == EditionConstants.National);
                    if (!ratesPrintPrivateDisplay.Any())
                        return model;
                }

                var pageNumber = ratesPrintPrivateDisplay.FirstOrDefault()?.EditionPageNo;

                if (!model.ExceptEditionPageNoFor_Rajshahi_Rangpur.Any(f=>f== pageNumber) &&  ratesPrintPrivateDisplay.Any(f => f.EditionId == EditionConstants.Rajshahi)
                    && ratesPrintPrivateDisplay.Any(f => f.EditionId == EditionConstants.Rangpur))
                {
                    ratesPrintPrivateDisplay = ratesPrintPrivateDisplay.Where(f => f.EditionId != EditionConstants.Rangpur);
                }

                foreach (var rate in ratesPrintPrivateDisplay)
                {
                    if (privateDisplay.IsColor)   //for color                 
                        estimatedCosting = rate.PerColumnInchColorRate * Convert.ToDecimal(privateDisplay.ColumnSize) * Convert.ToDecimal(privateDisplay.InchSize);
                    else  //for black and white  
                        estimatedCosting = rate.PerColumnInchBWRate * Convert.ToDecimal(privateDisplay.ColumnSize) * Convert.ToDecimal(privateDisplay.InchSize);
                    model.AdRate = model.AdRate + (privateDisplay.IsColor ? rate.PerColumnInchColorRate : rate.PerColumnInchBWRate);
                    model.OrderTotalAmount = model.OrderTotalAmount + estimatedCosting;
                }
            }
            else if (model.PrivateAdType == PrivateAdTypesConstants.Spot) //for spot
            {
                var ratesForSpotDisplay = await ratePrintSpotAdService.GetSpotAdRatesByEditionIdsAndEditionPage(model.EditionIds, model.EditionPageId);
                if (ratesForSpotDisplay == null || !ratesForSpotDisplay.Any()) return model;

                //get rates by default eidtion or national edition exist in editions list            
                if (model.NationalEdition)
                {
                    ratesForSpotDisplay = ratesForSpotDisplay.Where(f => f.EditionId == EditionConstants.National);
                    if (!ratesForSpotDisplay.Any())
                        return model;
                }

                var pageNumber = ratesForSpotDisplay.FirstOrDefault()?.EditionPageNo;

                if (!model.ExceptEditionPageNoFor_Rajshahi_Rangpur.Any(f => f == pageNumber) && ratesForSpotDisplay.Any(f => f.EditionId == EditionConstants.Rajshahi)
                    && ratesForSpotDisplay.Any(f => f.EditionId == EditionConstants.Rangpur))
                {
                    ratesForSpotDisplay = ratesForSpotDisplay.Where(f => f.EditionId != EditionConstants.Rangpur);
                }

                foreach (var rate in ratesForSpotDisplay)
                {
                    if (privateDisplay.IsColor)   //for color                 
                        estimatedCosting = rate.PerColumnInchColorRate * Convert.ToDecimal(privateDisplay.ColumnSize) * Convert.ToDecimal(privateDisplay.InchSize);
                    else  //for black and white  
                        estimatedCosting = rate.PerColumnInchBWRate * Convert.ToDecimal(privateDisplay.ColumnSize) * Convert.ToDecimal(privateDisplay.InchSize);

                    model.AdRate = model.AdRate + (privateDisplay.IsColor ? rate.PerColumnInchColorRate : rate.PerColumnInchBWRate);
                    model.OrderTotalAmount = model.OrderTotalAmount + estimatedCosting;
                }
            }
            else if (model.PrivateAdType == PrivateAdTypesConstants.EARPanel) //for EAR panel
            {
                var ratesForEarPanel = await ratePrintEarPanelAdService.GetEARPanelAdRatesByEditionIdsAndEditionPage(model.EditionIds, model.EditionPageId);

                if (ratesForEarPanel == null || !ratesForEarPanel.Any()) return model;

                //get rates by default eidtion or national edition exist in editions list            
                if (model.NationalEdition)
                {
                    ratesForEarPanel = ratesForEarPanel.Where(f => f.EditionId == EditionConstants.National);
                    if (!ratesForEarPanel.Any())
                        return model;
                }

                var pageNumber = ratesForEarPanel.FirstOrDefault()?.EditionPageNo;

                if (!model.ExceptEditionPageNoFor_Rajshahi_Rangpur.Any(f => f == pageNumber) && ratesForEarPanel.Any(f => f.EditionId == EditionConstants.Rajshahi)
                    && ratesForEarPanel.Any(f => f.EditionId == EditionConstants.Rangpur))
                {
                    ratesForEarPanel = ratesForEarPanel.Where(f => f.EditionId != EditionConstants.Rangpur);
                }

                foreach (var rate in ratesForEarPanel)
                {
                    estimatedCosting = rate.Rate; //* Convert.ToDecimal(privateDisplay.ColumnSize) * Convert.ToDecimal(privateDisplay.InchSize);
                    model.AdRate = model.AdRate + rate.Rate;
                    model.OrderTotalAmount = model.OrderTotalAmount + estimatedCosting;
                }
            }

            return model;
        }

        private List<ABPrintPrivateDisplayDetail> PopulateABPrintPrivateDisplayDetail(CheckoutPrivateDisplayViewModel model)
        {
            var listing = new List<ABPrintPrivateDisplayDetail>();

            foreach (var editionId in model.EditionIds)
            {
                var newABPrintPrivateDisplayDetail = new ABPrintPrivateDisplayDetail
                {
                    EditionId = editionId,
                    EditionPageId = model.EditionPageId
                };

                listing.Add(newABPrintPrivateDisplayDetail);
            }
            return listing;
        }

        private async Task GetDefaultPaymentModeAndDiscount(CheckoutPrivateDisplayViewModel model)
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
                .GetDetailsByAdTypeAndPaymentMode(ClassifiedTypeConstants.PrivateDisplay, paymentModeId);

            if (defaultDiscount != null)
                manualDiscountPercentage = defaultDiscount.DiscountRate;

            return manualDiscountPercentage;
        }

        #endregion
    }
}
