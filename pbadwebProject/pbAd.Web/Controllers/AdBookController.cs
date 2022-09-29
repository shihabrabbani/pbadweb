using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.Models;
using pbAd.Service.ABPrintClassifiedTexts;
using pbAd.Service.Advertisers;
using pbAd.Service.Agencies;
using pbAd.Service.Brands;
using pbAd.Service.Categories;
using pbAd.Service.EditionPages;
using pbAd.Service.OfferDates;
using pbAd.Service.RatePrintClassifiedTexts;
using pbAd.Service.SubCategories;
using pbAd.Web.Infrastructure.Helpers;
using pbAd.Web.ViewModels.AddBooks;
using pbAd.Web.ViewModels.Checkout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.Controllers
{
    public class AdBookController : AdminBaseController
    {
        #region Private Methods
        private readonly ICategoryService categoryService;
        private readonly IAdvertiserService advertiserService;
        private readonly ISubCategoryService subCategoryService;
        private readonly IABPrintClassifiedTextService classifiedTextService;
        private readonly IRatePrintClassifiedTextService ratePrintClassifiedTextService;
        private readonly IAgencyService agencyService;
        private readonly IBrandService brandService;
        private readonly IEditionPageService editionPageService;
        private readonly IOfferDateService offerDateService;

        #endregion

        #region Ctor

        public AdBookController(
             ICategoryService categoryService,
             IAdvertiserService advertiserService,
             ISubCategoryService subCategoryService,
             IABPrintClassifiedTextService aBPrintClassifiedTextService,
            IRatePrintClassifiedTextService ratePrintClassifiedTextService,
            IAgencyService agencyService,
            IOfferDateService offerDateService,
            IEditionPageService editionPageService,
            IBrandService brandService
            )
        {
            this.categoryService = categoryService;
            this.advertiserService = advertiserService;
            this.subCategoryService = subCategoryService;
            this.classifiedTextService = aBPrintClassifiedTextService;
            this.ratePrintClassifiedTextService = ratePrintClassifiedTextService;
            this.agencyService = agencyService;
            this.brandService = brandService;
            this.editionPageService = editionPageService;
            this.offerDateService = offerDateService;
        }

        #endregion

        #region Book Now

        public async Task<IActionResult> BookNow()
        {            
            var editionId = CurrentLoginUser.EditionId ?? 0;
            var ratePrintClassifiedText = await ratePrintClassifiedTextService.GetDefaultRatePrintClassifiedText(editionId);

            if (ratePrintClassifiedText == null || ratePrintClassifiedText.AutoId <= 0)
                return Redirect("/dashboard");

            //store rates into session
            HttpContext.Session.SetObject(SessionKeyConstants.RatePrintClassifiedText, ratePrintClassifiedText);

            var model = new BookAdViewModel
            {
                CategoryDropdownList = await GetCategoryDropdown(),
                RatePrintClassifiedText = ratePrintClassifiedText
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookNow(BookAdViewModel model)
        {
            ModelState.Remove("AdEnhancementType");
            ModelState.Remove("AgencyId");

            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                //validate form
                var reponseValidation = await ValidateForm(model);
                if (!reponseValidation.IsSuccess)
                    return Json(new { status = false, message = reponseValidation.Message, type = ActionResultTypeConstants.Message });

                model.DateBasedOfferList = model.DatesBasedOffer.Split(',');

                var bookingNo = CoreHelper.GenerateRandomToken(8);
                var currentDate = DateTime.Now;
                model.EditionId = CurrentLoginUser.EditionId ?? 0;

                var newAdvertiser = new Advertiser();
                if (model.Personal || CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
                {
                    model.RequiredAdvertiser = true;

                    //if ((model.AdvertiserContactNumber).Length != 11)
                    //    return Json(new { status = false, message = "Warning, Contact Number Must be 11 digits", type = ActionResultTypeConstants.Message });

                    //Get details by mobile
                   // var existingAdvertiser = await advertiserService.GetDetailsByMobile(model.AdvertiserContactNumber);
                    //Populate Advertiser info

                    //if (existingAdvertiser != null) 
                    //{ newAdvertiser = existingAdvertiser; 
                    //    newAdvertiser.AdvertiserName = model.AdvertiserName; }
                    //else 
                        newAdvertiser = PopulateAdvertiser(model, currentDate);
                }

                //populate ab print classified text
                var newABPrintClassifiedText = await PopulateABPrintClassifiedTextInfo(model, bookingNo, currentDate);

                //Populate ab print classified text detail
                var abPrintClassifiedTextDetailListing = await PopulateABPrintClassifiedTextDetail(model, currentDate);

                var adBookProcessRequest = new AdBookProcessModel
                {
                    RequiredAdvertiser = model.RequiredAdvertiser,
                    Advertiser = newAdvertiser,
                    ABPrintClassifiedText = newABPrintClassifiedText,
                    ABPrintClassifiedTextDetailListing = abPrintClassifiedTextDetailListing
                };

                //let's add display text info into database
                var response = await classifiedTextService.BookClassifiedTextAd(adBookProcessRequest);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/{response.Result.ABPrintClassifiedTextId}/payment/{response.Result.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Booking Edit

        [Route("/adbook/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(int id, string bookingno)
        {
            var bookingStep = BookingStepConstants.Booked;
            var classifiedText = await classifiedTextService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);
            if (classifiedText == null)
                return Redirect("/adbook/booknow");

            var classifiedTextDetailListing = await classifiedTextService.GetABPrintClassifiedTextDetailListing(id);

            if (!classifiedTextDetailListing.Any())
                return Redirect("/adbook/booknow");

            var editionId = CurrentLoginUser.EditionId ?? 0;
            var ratePrintClassifiedText = await ratePrintClassifiedTextService.GetDefaultRatePrintClassifiedText(editionId);

            if (ratePrintClassifiedText == null || ratePrintClassifiedText.AutoId <= 0)
                return Redirect("/dashboard");

            var agencyInfo = new Agency();
            var agency = await agencyService.GetDetailsById(classifiedText.AgencyId.Value);
            if (agency != null)
                agencyInfo = agency;

            var advertiserInfo = new Advertiser();
            var advertiser = await advertiserService.GetDetailsById(classifiedText.AdvertiserId);
            if (advertiser != null)
                advertiserInfo = advertiser;

            var basedOfferDates = classifiedTextDetailListing.Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();

            var basedOfferDatesInString = String.Join(", ", basedOfferDates);

            var adContent = "";

            var abPrintClassifiedTextDetail = classifiedTextDetailListing.FirstOrDefault();
            if (abPrintClassifiedTextDetail != null)
                adContent = abPrintClassifiedTextDetail.AdContent;

            //store rates into session
            HttpContext.Session.SetObject(SessionKeyConstants.RatePrintClassifiedText, ratePrintClassifiedText);

            var model = new BookAdViewModel
            {
                ABPrintClassifiedTextId = classifiedText.ABPrintClassifiedTextId,
                BookingNo = classifiedText.BookingNo,
                CategoryId = classifiedText.CategoryId,
                SubCategoryId = classifiedText.SubCategoryId,
                CategoryDropdownList = await GetCategoryDropdown(classifiedText.CategoryId),
                SubCategoryDropdownList = await GetSubCategoryDropdown(classifiedText.CategoryId, classifiedText.SubCategoryId),
                AdContent = adContent,
                BasedOfferDates = basedOfferDates,
                DatesBasedOffer = basedOfferDatesInString,
                AgencyInfo = agencyInfo,
                AgencyId = agencyInfo.AgencyId,
                AdvertiserInfo = advertiserInfo,
                EstimatedTotal = classifiedText.GrossTotal,
                AgencyAutoComplete = agencyInfo.AgencyName,
                AdvertiserName = advertiserInfo.AdvertiserName,
                AdvertiserContactNumber = advertiserInfo.AdvertiserMobileNo,
                Personal = advertiserInfo.AdvertiserId > 0,
                RatePrintClassifiedText = ratePrintClassifiedText,
                Remarks= classifiedText.Remarks
            };

            //populate adenhancement type with bullet
            PopulateAdEnhancementTypeWithBullet(classifiedText, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/adbook/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(BookAdViewModel model)
        {
            ModelState.Remove("AdEnhancementType");
            ModelState.Remove("AgencyId");

            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                //validate form
                var reponseValidation = await ValidateForm(model);
                if (!reponseValidation.IsSuccess)
                    return Json(new { status = false, message = reponseValidation.Message, type = ActionResultTypeConstants.Message });

                model.DateBasedOfferList = model.DatesBasedOffer.Split(',');

                var randomNo = model.BookingNo;
                var currentDate = DateTime.Now;
                model.EditionId = CurrentLoginUser.EditionId ?? 0;

                var newAdvertiser = new Advertiser();
                if (model.Personal || CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
                {
                    model.RequiredAdvertiser = true;

                    //if ((model.AdvertiserContactNumber).Length != 11)
                    //    return Json(new { status = false, message = "Warning, Contact Number Must be 11 digits", type = ActionResultTypeConstants.Message });

                    //Get details by mobile
                    //var existingAdvertiser = await advertiserService.GetDetailsByMobile(model.AdvertiserContactNumber);
                    ////Populate Advertiser info

                    //if (existingAdvertiser != null) { newAdvertiser = existingAdvertiser; newAdvertiser.AdvertiserName = model.AdvertiserName; }
                    //else 
                        
                        newAdvertiser = PopulateAdvertiser(model, currentDate);
                }

                //populate ab print classified text
                var newClassifiedText = await PopulateABPrintClassifiedTextInfo(model, model.BookingNo, currentDate);
                newClassifiedText.ABPrintClassifiedTextId = model.ABPrintClassifiedTextId;
                newClassifiedText.ModifiedBy = CurrentLoginUser.UserId;

                //Populate ab print classified text detail
                var classifiedTextDetailListing = await PopulateABPrintClassifiedTextDetail(model, currentDate);

                var adBookProcessRequest = new AdBookProcessModel
                {
                    RequiredAdvertiser = model.RequiredAdvertiser,
                    Advertiser = newAdvertiser,
                    ABPrintClassifiedText = newClassifiedText,
                    ABPrintClassifiedTextDetailListing = classifiedTextDetailListing
                };

                //let's add display text info into database
                var response = await classifiedTextService.EditBookedClassifiedTextAd(adBookProcessRequest);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/{response.Result.ABPrintClassifiedTextId}/payment/{response.Result.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Ajax Calls

        public async Task<JsonResult> GetSubCategoryList(int categoryId)
        {
            var subCategories = await subCategoryService.GetSubCategoryByCategory(categoryId);
            var defaultSelectionList = new List<SelectListItem> { new SelectListItem { Text = "Select Sub Category", Value = "" } };

            if (subCategories.Any())
            {
                var dbList = subCategories.Select(s => new SelectListItem { Text = s.SubCategoryName, Value = s.SubCategoryId.ToString() });
                defaultSelectionList.AddRange(dbList);
            }

            return Json(defaultSelectionList);
        }

        [HttpPost]
        public async Task<JsonResult> GetDateOffer(string[] offerDates)
        {
            if (offerDates == null || !offerDates.Any())
                return Json(null);

            //fatch date offer
            var offerDate = await FatchDateOffer(offerDates);

            return Json(offerDate);
        }

        [HttpPost]
        public async Task<JsonResult> AgencyAutoComplete(string searchKey)
        {
            try
            {
                var filter = new AgencySearchFilter
                {
                    SearchTerm = searchKey ?? string.Empty
                };

                //filter agency by search filter
                var agencyListing = await agencyService.GetAgencyForAutoComplete(filter);
                var formattedAgencies = agencyListing.Select(b => new
                {
                    AgencyId = b.AgencyId,
                    AgencyName = b.AgencyName
                });

                return Json(new { Data = formattedAgencies });
            }
            catch (Exception e)
            {
                return Json(new { Data = "" });
            }
        }

        #endregion

        #region Private Methods

        private async Task<List<ABPrintClassifiedTextDetail>> PopulateABPrintClassifiedTextDetail(BookAdViewModel model, DateTime currentDate)
        {
            var editionPage = await editionPageService.GetEdtionPageByEditionAndPageNo(model.EditionId, EditionPagesConstants.Page_6);

            var listing = new List<ABPrintClassifiedTextDetail>();

            foreach (var date in model.DateBasedOfferList)
            {
                ABPrintClassifiedTextDetail newABPrintClassifiedTextDetail =
                    new ABPrintClassifiedTextDetail
                    {
                        AdContent = model.AdContent,
                        EditionId = model.EditionId,
                        EditionPageId = editionPage?.EditionPageId,
                        PublishDate = Convert.ToDateTime(date)
                    };

                listing.Add(newABPrintClassifiedTextDetail);
            }
            return listing;
        }

        public EstimatedTotalCalculationViewModel FetchEstimatedTotalAmount(EstimatedTotalCalculationViewModel model)
        {
            //get rates from session
            var rateClassifiedText = HttpContext.Session.GetObject<RatePrintClassifiedText>(SessionKeyConstants.RatePrintClassifiedText);
            model.EstimatedTotalAmount = 0;

            if (rateClassifiedText == null || rateClassifiedText.AutoId <= 0)
                return model;

            if (string.IsNullOrWhiteSpace(model.AdContent))
                return model;

            //get total word counts
            model.TotalWordCount = CoreHelper.TotalWordCounts(model.AdContent);

            //get base amount
            decimal baseAmount = model.TotalWordCount * rateClassifiedText.PerWordRate;
            decimal aditionalAmountBullet = 0;
            decimal aditionalAmount = 0;

            //for bullet related
            if (model.IsBigBulletPointSingle)
            {
                aditionalAmountBullet = (baseAmount * rateClassifiedText.BigBulletPointSingle) / 100;
            }
            if (model.IsBigBulletPointDouble)
            {
                aditionalAmountBullet = (baseAmount * rateClassifiedText.BigBulletPointDouble) / 100;
            }

            //ad enhancetype wise 
            if (model.IsBold)
            {
                aditionalAmount = (baseAmount * rateClassifiedText.BoldPercentage) / 100;
            }
            if (model.IsBoldinScreen)
            {
                aditionalAmount = (baseAmount * rateClassifiedText.BoldinScreenPercentage) / 100;
            }
            if (model.IsBoldScreenSingleBox)
            {
                aditionalAmount = (baseAmount * rateClassifiedText.BoldScreenSingleBoxPercentage) / 100;
            }
            if (model.IsBoldScreenDoubleBox)
            {
                aditionalAmount = (baseAmount * rateClassifiedText.BoldScreenDoubleBoxPercentage) / 100;
            }

            //get estimated amount
            model.EstimatedTotalAmount = baseAmount + aditionalAmountBullet + aditionalAmount;

            return model;
        }

        private async Task<ABPrintClassifiedText> PopulateABPrintClassifiedTextInfo(BookAdViewModel model, string bookingNo, DateTime currentDate)
        {
            var newABPrintClassifiedText = new ABPrintClassifiedText();

            //Populate adenhancement type
            PopulateAdEnhancementType(model, newABPrintClassifiedText);

            var estimatedTotalCalModel = new EstimatedTotalCalculationViewModel
            {
                AdContent = model.AdContent,

                IsBigBulletPointSingle = newABPrintClassifiedText.IsBigBulletPointSingle,
                IsBigBulletPointDouble = newABPrintClassifiedText.IsBigBulletPointDouble,

                IsBold = newABPrintClassifiedText.IsBold,
                IsBoldinScreen = newABPrintClassifiedText.IsBoldinScreen,
                IsBoldScreenSingleBox = newABPrintClassifiedText.IsBoldScreenSingleBox,
                IsBoldScreenDoubleBox = newABPrintClassifiedText.IsBoldScreenDoubleBox
            };

            //calculate estimated amount   
            var estimatedTotalCalculation = FetchEstimatedTotalAmount(estimatedTotalCalModel);

            decimal discountPercentage = 0;
            int offerDateId = 0;

            //fatch date offer
            var offerDate = await FatchDateOffer(model.DateBasedOfferList);
            if (offerDate != null)
            {
                offerDateId = offerDate.OfferDateId;
                discountPercentage = offerDate.DiscountPercentage;
            }

            int totalPublishDates = model.DateBasedOfferList.Count();

            decimal grossTotal = Convert.ToDecimal(totalPublishDates) * estimatedTotalCalculation.EstimatedTotalAmount;

            newABPrintClassifiedText.BookingNo = bookingNo;
            newABPrintClassifiedText.CategoryId = model.CategoryId;
            newABPrintClassifiedText.SubCategoryId = model.SubCategoryId;
            newABPrintClassifiedText.TotalCount = model.TotalWordCount;
            newABPrintClassifiedText.BookedBy = CurrentLoginUser.UserId;
            newABPrintClassifiedText.BookDate = currentDate;
            newABPrintClassifiedText.AdStatus = AdStatusConstants.Booked;
            newABPrintClassifiedText.GrossTotal = grossTotal;
            newABPrintClassifiedText.DiscountPercent = discountPercentage;
            newABPrintClassifiedText.OfferDateId = offerDateId;
            newABPrintClassifiedText.UpazillaId = CurrentLoginUser.UpazillaId;
            newABPrintClassifiedText.AgencyId = model.AgencyId;
            newABPrintClassifiedText.BrandId = model.BrandId;
            newABPrintClassifiedText.BookingStep = BookingStepConstants.Booked;
            newABPrintClassifiedText.CreatedBy = CurrentLoginUser.UserId;
            newABPrintClassifiedText.IsCorrespondent = CurrentLoginUser.IsCorrespondentUser;
            newABPrintClassifiedText.Remarks = model.Remarks;

            newABPrintClassifiedText.DiscountAmount = (newABPrintClassifiedText.GrossTotal * newABPrintClassifiedText.DiscountPercent) / 100;
            newABPrintClassifiedText.NetAmount = newABPrintClassifiedText.GrossTotal - newABPrintClassifiedText.DiscountAmount;

            decimal vat = 0;
            if (model.EditionId == EditionConstants.National) vat = 15;

            newABPrintClassifiedText.VATAmount = (newABPrintClassifiedText.NetAmount * vat) / 100;
            newABPrintClassifiedText.Commission = 0;

            //agency commission
            if (!model.Personal && model.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                if (agency != null && agency.AgencyId > 0)
                    newABPrintClassifiedText.Commission = agency.CTCommission;
            }
            else if (CurrentLoginUser.IsCorrespondentUser)
            {
                newABPrintClassifiedText.Commission = CurrentLoginUser.DefaultCommission;
            }

            return newABPrintClassifiedText;
        }

        private void PopulateAdEnhancementType(BookAdViewModel model, ABPrintClassifiedText newABPrintClassifiedText)
        {
            if (model.AdEnhancementTypeBullet == AddEnhancementTypeConstants.BigBulletPointSingle)
            {
                newABPrintClassifiedText.IsBigBulletPointSingle = true;
            }
            if (model.AdEnhancementTypeBullet == AddEnhancementTypeConstants.BigBulletPointDouble)
            {
                newABPrintClassifiedText.IsBigBulletPointDouble = true;
            }


            if (model.AdEnhancementType == AddEnhancementTypeConstants.Bold)
            {
                newABPrintClassifiedText.IsBold = true;
            }
            if (model.AdEnhancementType == AddEnhancementTypeConstants.BoldInScreen)
            {
                newABPrintClassifiedText.IsBoldinScreen = true;
            }
            if (model.AdEnhancementType == AddEnhancementTypeConstants.BoldInScreenAndSingleBox)
            {
                newABPrintClassifiedText.IsBoldScreenSingleBox = true;
            }
            if (model.AdEnhancementType == AddEnhancementTypeConstants.BoldInScreenAndDoubleBoxes)
            {
                newABPrintClassifiedText.IsBoldScreenDoubleBox = true;
            }
        }

        private void PopulateAdEnhancementTypeWithBullet(ABPrintClassifiedText classifiedText, BookAdViewModel model)
        {
            if (classifiedText.IsBigBulletPointSingle)
            {
                model.AdEnhancementTypeBullet = AddEnhancementTypeConstants.BigBulletPointSingle;
            }
            if (classifiedText.IsBigBulletPointDouble)
            {
                model.AdEnhancementTypeBullet = AddEnhancementTypeConstants.BigBulletPointDouble;
            }

            if (classifiedText.IsBold)
            {
                model.AdEnhancementType = AddEnhancementTypeConstants.Bold;
            }
            if (classifiedText.IsBoldinScreen)
            {
                model.AdEnhancementType = AddEnhancementTypeConstants.BoldInScreen;
            }
            if (classifiedText.IsBoldScreenSingleBox)
            {
                model.AdEnhancementType = AddEnhancementTypeConstants.BoldInScreenAndSingleBox;
            }
            if (classifiedText.IsBoldScreenDoubleBox)
            {
                model.AdEnhancementType = AddEnhancementTypeConstants.BoldInScreenAndDoubleBoxes;
            }
        }

        private Advertiser PopulateAdvertiser(BookAdViewModel model, DateTime currentDate)
        {
            return new Advertiser
            {
                AdvertiserName = model.AdvertiserName,
                AdvertiserType = 3, //
                AdvertiserEmail = "N/A",
                AdvertiserMobileNo = model.AdvertiserContactNumber,
                CreatedBy = CurrentLoginUser.UserId,
                CreatedDate = currentDate
            };
        }

        private async Task<IEnumerable<SelectListItem>> GetCategoryDropdown(int selected = 0)
        {
            var filter = new CategorySearchFilter
            {
                CategoryType = 2
            };

            var leadSource = await categoryService.GetCategoryListForDropdown(filter);

            return leadSource.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = selected == Convert.ToInt32(i.Value)
                }).ToList();
        }

        private async Task<IEnumerable<SelectListItem>> GetBrandDropdown(int selected = 0)
        {
            var leadSource = await brandService.GetBrandListForDropdown();

            return leadSource.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = selected == Convert.ToInt32(i.Value)
                }).ToList();
        }

        private async Task<IEnumerable<SelectListItem>> GetAgencyDropdown(int selected = 0)
        {
            var leadSource = await agencyService.GetAgencyListForDropdown();

            return leadSource.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = selected == Convert.ToInt32(i.Value)
                }).ToList();
        }

        private async Task<IEnumerable<SelectListItem>> GetSubCategoryDropdown(int categoryId, int selected = 0)
        {
            var leadSource = await subCategoryService.GetSubCategoryListForDropdown(categoryId);

            return leadSource.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = selected == Convert.ToInt32(i.Value)
                }).ToList();
        }

        public async Task<OfferDate> FatchDateOffer(string[] offerDates)
        {
            try
            {
                if (offerDates == null || !offerDates.Any())
                    return new OfferDate();

                string[] dateBasedOfferList = offerDates;

                var datesBasedOffer = new List<DateTime>();
                foreach (var item in dateBasedOfferList) datesBasedOffer.Add(Convert.ToDateTime(item));

                var noOfTimes = datesBasedOffer.Count;

                //Get by days range and no of time
                var offerDate = await offerDateService.GetByDaysRangeAndNoofTime(datesBasedOffer, noOfTimes);

                return offerDate;
            }
            catch
            {
                return new OfferDate();
            }
        }

        private async Task<BaseResponse> ValidateForm(BookAdViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AdContent))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Ad Matter is Required" };

                var totalWords = model.AdContent.Split(" ");
                var count = 0;
                foreach (var item in totalWords)
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    count = count + 1;
                }

                if (count > 50)
                {
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Ad Matter Exceeds 50 Words" };
                }

                if (string.IsNullOrEmpty(model.DatesBasedOffer))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Please add atleast DATE BASED OFFER" };

                if ((model.Personal || CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
                   && ( string.IsNullOrWhiteSpace(model.AdvertiserName))
                   )
                    return new BaseResponse { IsSuccess = false, Message = "You must fill advitiser name." };


                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) &&
                    (
                        (string.IsNullOrWhiteSpace(model.AgencyAutoComplete) || model.AgencyId <= 0) &&
                        (string.IsNullOrWhiteSpace(model.AdvertiserName))
                    ))
                    return new BaseResponse { IsSuccess = false, Message = "You must fill Agency or Advertiser" };


                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) && (!string.IsNullOrWhiteSpace(model.AgencyAutoComplete) && model.AgencyId > 0))
                {
                    var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                    if (agency == null || agency.AgencyId <= 0)
                        return new BaseResponse { IsSuccess = false, Message = "Agency info modified. Please select agent again!" };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = "There on validating form" };
            }

            return new BaseResponse { IsSuccess = true, Message = "Success" };
        }


        #endregion
    }
}
