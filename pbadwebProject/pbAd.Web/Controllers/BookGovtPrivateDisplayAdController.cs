#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABPrintPrivateDisplays;
using pbAd.Data.Models;
using pbAd.Service.ABPrintPrivateDisplays;
using pbAd.Service.Advertisers;
using pbAd.Service.Agencies;

using pbAd.Service.Brands;
using pbAd.Service.Categories;
using pbAd.Service.EditionPages;
using pbAd.Service.Editions;
using pbAd.Service.OfferDates;
using pbAd.Service.RatePrintGovtAds;
using pbAd.Service.RatePrintPrivateDisplays;
using pbAd.Service.RatePrintSpotAds;
using pbAd.Service.SubCategories;
using pbAd.Web.ViewModels.BookPrivateDisplayAds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Web.Controllers
{
    public class BookGovtPrivateDisplayAdController : AdminBaseController
    {
        #region Private Methods
        private readonly ICategoryService categoryService;
        private readonly IAdvertiserService advertiserService;
        private readonly ISubCategoryService subCategoryService;
        private readonly IABPrintPrivateDisplayService abPrintPrivateDisplayGovtService;
        private readonly IRatePrintGovtAdService ratePrintGovtAdService;
        private readonly IAgencyService agencyService;
        private readonly IBrandService brandService;
        private readonly IEditionService editionService;
        private readonly IEditionPageService editionPageService;
        private readonly IOfferDateService offerDateService;

        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        #endregion

        #region Ctor

        public BookGovtPrivateDisplayAdController(
             ICategoryService categoryService,
             IAdvertiserService advertiseService,
             ISubCategoryService subCategoryService,
             IABPrintPrivateDisplayService abPrintPrivateDisplayGovtService,
             IRatePrintGovtAdService ratePrintGovtAdService,
             IOfferDateService offerDateService,
             IAgencyService agencyService,
             IBrandService brandService,
             IEditionService editionService,
      
                 Microsoft.Extensions.Configuration.IConfiguration configuration,
             IEditionPageService editionPageService
            )
        {
            this.categoryService = categoryService;
            this.advertiserService = advertiseService;
            this.subCategoryService = subCategoryService;
            this.abPrintPrivateDisplayGovtService = abPrintPrivateDisplayGovtService;
            this.ratePrintGovtAdService = ratePrintGovtAdService;
            this.agencyService = agencyService;
            this.brandService = brandService;
            this.editionService = editionService;
            this.editionPageService = editionPageService;
            this.offerDateService = offerDateService;
           
            this.configuration = configuration;
        }

        #endregion

        #region Book Now
        public async Task<IActionResult> BookNow()
        {
            /*
            var editionId = CurrentLoginUser.EditionId ?? 0;
            var ratePrintClassifiedText = await ratePrintGovtAdService.GetDefaultRate(editionId);

            if (ratePrintClassifiedText == null || ratePrintClassifiedText.AutoId <= 0)
                return Redirect("/dashboard");
            */

            var model = new BookPrivateDisplayGovtAdViewModel
            {
                EditionPageDropdownList = await GetEditionPageDropdown(),
                ColumnSizeDropdownList = GetColumnSizeDropdownList(),
                InchSize=1,
                ColumnSize=1
            };

            if (CurrentLoginUser.IsCorrespondentUser)
            {
                model.InsideDhaka = true;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BookNow(BookPrivateDisplayGovtAdViewModel model)
        {
            model.InsideDhaka = true;
            model.UploadLater = true;
            ModelState.Remove("AgencyId"); ModelState.Remove("AdvertiserId");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                //validate form
                var responseValidateForm = await ValidateForm(model);
                if (!responseValidateForm.IsSuccess)
                    return Json(new { status = false, message = responseValidateForm.Message, type = ActionResultTypeConstants.Message });

                var bookingNo = CoreHelper.GenerateRandomToken(8);
                var currentDate = DateTime.Now;
                
                //Populate Advertiser info
                var newAdvertiser = new Advertiser();
                var filter = new AdvertiserSearchFilter
                {
                    AdvertiserId = model.AdvertiserId,
                    MobileNo = model.AdvertiserName
                };
               // Get details by mobile
                var existingAdvertiser = await advertiserService.GetAdvirtizerInfoByFilter(filter);

                //Populate Advertiser info
                if (existingAdvertiser != null)
                {
                    newAdvertiser = existingAdvertiser;
                    newAdvertiser.AdvertiserName = model.AdvertiserName;
                    newAdvertiser.AdvertiserMobileNo = model.AdvertiserContactNumber;
                }
                else
                {
                    newAdvertiser = PopulateAdvertiser(model, currentDate);
                }
                   

                var privateDisplayMediaContents = new List<PrivateDisplayMediaContent>();

                if (!model.UploadLater)//let's save file & populate private display media content
                    privateDisplayMediaContents = null;

                model.DateBasedOfferList = model.DatesBasedOffer.Split(',');

                //populate private display
                var newPrivateDisplay = await PopulatePrivateDisplayInfo(model, bookingNo, currentDate);

                //Populate private display detail
                var privateDisplayDetailListing = PopulatePrivateDisplayDetail(model, currentDate);

                var request = new UploadLaterAdProcessModel
                {
                    RequiredAdvertiser = true,
                    Advertiser = newAdvertiser,
                    ABPrintPrivateDisplay = newPrivateDisplay,
                    ABPrintPrivateDisplayDetailListing = privateDisplayDetailListing,
                    PrivateDisplayMediaContents = privateDisplayMediaContents,
                    
                };

                //let's book private display ad
                var response = await abPrintPrivateDisplayGovtService.BookPrivateDisplayAd(request);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/govtad/{response.Result.ABPrintPrivateDisplayId}/payment/{response.Result.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Booking Edit
        [Route("/bookgovtprivatedisplayad/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(int id, string bookingno)
        {
            //get private display
            var bookingStep = BookingStepConstants.Booked;
            var privateDisplay = await abPrintPrivateDisplayGovtService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);
            if (privateDisplay == null)
                return Redirect("/BookGovtPrivateDisplayAd/booknow");

            //get private display detail listings
            var privateDisplayDetailListing = await abPrintPrivateDisplayGovtService.GetABPrintPrivateDisplayDetailListing(id);

            if (!privateDisplayDetailListing.Any())
                return Redirect("/BookGovtPrivateDisplayAd/booknow");

            //get media content listings
            var displayMediaContentListing = await abPrintPrivateDisplayGovtService.GetPrivateDisplayMediaContentListing(id);

            //get editions
            var edtions = await editionService.GetAllEditions();

            //get publish date as base offer dates
            var basedOfferDates = privateDisplayDetailListing.Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();

            var basedOfferDatesInString = String.Join(", ", basedOfferDates);

            //get default editions
            var defaultEdtion = edtions.FirstOrDefault(f => f.EditionId == CurrentLoginUser.EditionId);

            //editions except default edition
            var filteredEditions = edtions.Where(f => f.EditionId != CurrentLoginUser.EditionId).ToList();

            var privateDisplayDetail = privateDisplayDetailListing.FirstOrDefault();

            var editionPage = await editionPageService.GetDetailsById(privateDisplayDetail.EditionPageId);

            var brand = await brandService.GetDetailsById(privateDisplay.BrandId.Value);
            if (brand == null) brand = new Brand();

            var agency = await agencyService.GetDetailsById(privateDisplay.AgencyId.Value);
            if (agency == null) agency = new Agency();

            var advertiser = await advertiserService.GetDetailsById(privateDisplay.AdvertiserId);
            if (advertiser == null) advertiser = new Advertiser();

            var model = new BookPrivateDisplayGovtAdViewModel
            {
                EditionPageDropdownList = await GetEditionPageDropdown(privateDisplayDetail.EditionPageId),
                BrandAutoComplete = brand.BrandName,
                BrandId = brand.BrandId,
                AgencyAutoComplete = agency.AgencyName,
                AgencyId = agency.AgencyId,
                AdvertiserName = advertiser.AdvertiserName,
                AdvertiserContactNumber = advertiser.AdvertiserMobileNo,
                AdvertiserId = advertiser.AdvertiserId,
                IsColor = privateDisplay.IsColor,
                //Corporation = privateDisplay.Corporation,
                DatesBasedOffer = basedOfferDatesInString,
                UploadLater = privateDisplay.UploadLater,
                EditionPageId = privateDisplayDetail.EditionPageId,
                EstimatedTotal = privateDisplay.GrossTotal,
                ColumnSize = privateDisplay.ColumnSize,
                InchSize = privateDisplay.InchSize,
                CategoryId = privateDisplay.CategoryId,
                Remarks = privateDisplay.Remarks,
                ABPrintPrivateDisplayId = privateDisplay.ABPrintPrivateDisplayId,
                ColumnSizeDropdownList = GetColumnSizeDropdownList(privateDisplay.ColumnSize),
                Corporation = privateDisplay.Corporation,
                InsideDhaka = privateDisplay.InsideDhaka,
            };

            if (!model.UploadLater)
            {
                var mediaContents = await abPrintPrivateDisplayGovtService.GetMediaContentsByPrivateDisplayId(privateDisplay.ABPrintPrivateDisplayId);
                model.MediaContents = mediaContents;
            }

            return View(model);
        }

        [HttpPost]
        [Route("/bookgovtprivatedisplayad/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(BookPrivateDisplayGovtAdViewModel model)
        {
            ModelState.Remove("AgencyId"); ModelState.Remove("AdvertiserId");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                //validate form
                var responseValidateForm = await ValidateEditForm(model);
                if (!responseValidateForm.IsSuccess)
                    return Json(new { status = false, message = responseValidateForm.Message, type = ActionResultTypeConstants.Message });

                var randomNo = CoreHelper.GenerateRandomToken(8);
                var currentDate = DateTime.Now;

                //Populate Advertiser info
                var newAdvertiser = new Advertiser();

                ////Get details by mobile
                var existingAdvertiser = await advertiserService.GetAdvirtizerInfo(model.AdvertiserId, model.AdvertiserName, model.Personal);

                //Populate Advertiser info
                if (existingAdvertiser != null) 
                { newAdvertiser = existingAdvertiser; 
                    newAdvertiser.AdvertiserName = model.AdvertiserName; }
                else
                {
                    newAdvertiser = PopulateAdvertiser(model, currentDate);
                }

                   

                var privateDisplayMediaContents = new List<PrivateDisplayMediaContent>();
                if (!model.UploadLater && model.ImageContents != null)//let's save file & populate private display media content
                    privateDisplayMediaContents = null;

                model.DateBasedOfferList = model.DatesBasedOffer.Split(',');

                //populate private display
                var newPrivateDisplay = await PopulatePrivateDisplayInfo(model, model.BookingNo, currentDate);
                newPrivateDisplay.ABPrintPrivateDisplayId = model.ABPrintPrivateDisplayId;
                //Populate private display detail
                var privateDisplayDetailListing = PopulatePrivateDisplayDetail(model, currentDate);

                var request = new UploadLaterAdProcessModel
                {
                    RequiredAdvertiser = true,
                    Advertiser = newAdvertiser,
                    RemoveExistingFiles = model.RemoveExistingFiles,
                    ABPrintPrivateDisplay = newPrivateDisplay,
                    ABPrintPrivateDisplayDetailListing = privateDisplayDetailListing,
                    PrivateDisplayMediaContents = privateDisplayMediaContents
                };

                //let's updated private display ad
                var response = await abPrintPrivateDisplayGovtService.EditPrivateDisplayBook(request);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/govtad/{response.Result.ABPrintPrivateDisplayId}/payment/{response.Result.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Ajax Calls

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

        [HttpPost]
        public async Task<JsonResult> BrandAutoComplete(string searchKey)
        {
            try
            {
                var filter = new BrandSearchFilter
                {
                    SearchTerm = searchKey ?? string.Empty
                };

                //filter brand by search filter
                var brandListing = await brandService.GetBrandForAutoComplete(filter);
                var formattedAgencies = brandListing.Select(b => new
                {
                    BrandId = b.BrandId,
                    BrandName = b.BrandName
                });

                return Json(new { Data = formattedAgencies });
            }
            catch (Exception e)
            {
                return Json(new { Data = "" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetEstimatedCosting(PrivateDisplayGovtEstimatedCostingViewModel model)
        {
            try
            {
                //fetch estimated total amount
                var estimatedCostingInfo = await FetchEstimatedTotalAmount(model);

                return Json(new { status = true, estimatedCosting = estimatedCostingInfo, type = ActionResultTypeConstants.Message });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "Warning, Error on getting estimated costing. Please contact with support.", type = ActionResultTypeConstants.Message });
            }
        }

        public async Task<JsonResult> GetBrandWiseAdvirtiser(int brandId)
        {
            var filter = new BrandSearchFilter
            {
                BrandId = brandId
            };

            var brandRelation = await brandService.GetBrandWiseAdvirtiser(filter);

            return Json(brandRelation);
        }
                
        public async Task<JsonResult> GetCategoryList(int brandId)
        {
            var defaultSelectionList = new List<SelectListItem> { new SelectListItem { Text = "Select Category", Value = "" } };

            //if brand wise category found found then populate ddl
            var categories = await categoryService.GetCategoryListForDropdownByBrandId(brandId);
            if (categories.Any())
            {
                var newCategories =
                        from c in categories
                        group c by new
                        {
                            c.Text,
                            c.Value
                        } into gcs
                        select new SelectListItem()
                        {
                            Text = gcs.Key.Text,
                            Value = gcs.Key.Value
                        };

                var dataWithBrandList = newCategories;
                defaultSelectionList.AddRange(dataWithBrandList);

                return Json(defaultSelectionList);
            }

            //if brand wise category not found found then populate ddl

            var filter = new CategorySearchFilter { CategoryType = 1 };
            var leadSource = await categoryService.GetCategoryListForDropdown(filter);

            var dbList = leadSource.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text
                }).ToList();

            defaultSelectionList.AddRange(dbList);

            return Json(defaultSelectionList);
        }

        [HttpPost]
        public async Task<JsonResult> AdvertiserAutoComplete(string searchKey)
        {
            try
            {
                var filter = new AdvertiserSearchFilter
                {
                    SearchTerm = searchKey ?? string.Empty,
                    AdvertiserType = 1
                };

                //filter advertiser by search filter
                var advertisersListing = await advertiserService.GetAdvertiserForAutoComplete(filter);
                var formattedAdvertisers = advertisersListing.Select(b => new
                {
                    AdvertiserId = b.AdvertiserId,
                    AdvertiserName = b.AdvertiserName
                });

                return Json(new { Data = formattedAdvertisers });
            }
            catch (Exception e)
            {
                return Json(new { Data = "" });
            }
        }

        #endregion

        #region Private Methods

        public List<SelectListItem> GetColumnSizeDropdownList(int columnSize = 0)
        {
            var defaultSelectionList = new List<SelectListItem> { };

            for (int i = 1; i <= 8; i++)
            {
                var selectListItem = new SelectListItem { Text = i.ToString(), Value = i.ToString() };
                defaultSelectionList.Add(selectListItem);
            }

            return defaultSelectionList;
        }

        private async Task<BaseResponse> ValidateEditForm(BookPrivateDisplayGovtAdViewModel model)
        {
            try
            {
                //if (model.Personal && (string.IsNullOrWhiteSpace(model.AdvertiserContactNumber) || string.IsNullOrWhiteSpace(model.AdvertiserName)))
                //    return new BaseResponse { IsSuccess = false, Message = "You must fill advitiser name and contact number." };

                //if (model.Personal && ((model.AdvertiserContactNumber).Length != 11))
                //    return new BaseResponse { IsSuccess = false, Message = "Warning! Contact Number Must be 11 digits" };

                if (CurrentLoginUser.IsCorrespondentUser && model.AdvertiserId <= 0 && (string.IsNullOrWhiteSpace(model.AdvertiserContactNumber)))
                    return new BaseResponse { IsSuccess = false, Message = "You must fill advitiser contact number." };

                if (!string.IsNullOrWhiteSpace(model.AdvertiserContactNumber) && model.AdvertiserContactNumber.Length != 11)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Contact Number Must be 11 digits" };

                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) && (string.IsNullOrWhiteSpace(model.AgencyAutoComplete) || model.AgencyId <= 0))
                    return new BaseResponse { IsSuccess = false, Message = "Warning, Agency is Rrequired" };

                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) && (!string.IsNullOrWhiteSpace(model.AgencyAutoComplete) && model.AgencyId > 0))
                {
                    var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                    if (agency == null || agency.AgencyId <= 0)
                        return new BaseResponse { IsSuccess = false, Message = "Warning! Agency info modified. Please select Agency again!" };
                }

                if (model.ColumnSize <= 0 || model.InchSize <= 0)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Please enter the valid input for Column Size and Inch Size." };

                if (string.IsNullOrEmpty(model.DatesBasedOffer))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Please add atleast DATE BASED OFFER" };

                if (model.InchSize > 20)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Inch size exceed the value of 20" };

                if (!model.UploadLater && !model.IsFoundExistingFiles && (model.ImageContents == null || !model.ImageContents.Any()))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Uploaded file not found." };

                var modelEstimatedCosting = new PrivateDisplayGovtEstimatedCostingViewModel
                {
                    ColumnSize = model.ColumnSize,
                    InchSize = model.InchSize,
                    IsColor = model.IsColor,
                    Corporation = model.Corporation,
                    EditionPageId = model.EditionPageId
                };

                //fetch estimated total amount
                var estimatedCostingInfo = await FetchEstimatedTotalAmount(modelEstimatedCosting);

                if (estimatedCostingInfo.IsRateNotFound || estimatedCostingInfo.EstimatedCosting <= 0)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Rate not found. Please try another edition page." };

                return new BaseResponse { IsSuccess = true, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        private async Task<BaseResponse> ValidateForm(BookPrivateDisplayGovtAdViewModel model)
        {
            try
            {
                //if (model.Personal && (string.IsNullOrWhiteSpace(model.AdvertiserContactNumber) || string.IsNullOrWhiteSpace(model.AdvertiserName)))
                //    return new BaseResponse { IsSuccess = false, Message = "You must fill advitiser name and contact number." };

                //if (model.Personal && ((model.AdvertiserContactNumber).Length != 11))
                //    return new BaseResponse { IsSuccess = false, Message = "Warning! Contact Number Must be 11 digits" };

                if (CurrentLoginUser.IsCorrespondentUser && model.AdvertiserId <= 0 && (string.IsNullOrWhiteSpace(model.AdvertiserContactNumber)))
                    return new BaseResponse { IsSuccess = false, Message = "You must fill advitiser contact number." };

                if (!string.IsNullOrWhiteSpace(model.AdvertiserContactNumber) && model.AdvertiserContactNumber.Length != 11)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Contact Number Must be 11 digits" };

                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) && (string.IsNullOrWhiteSpace(model.AgencyAutoComplete) || model.AgencyId <= 0))
                    return new BaseResponse { IsSuccess = false, Message = "Warning, Agency is Rrequired" };

                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) && (!string.IsNullOrWhiteSpace(model.AgencyAutoComplete) && model.AgencyId > 0))
                {
                    var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                    if (agency == null || agency.AgencyId <= 0)
                        return new BaseResponse { IsSuccess = false, Message = "Warning! Agency info modified. Please select Agency again!" };
                }

                if (model.ColumnSize <= 0 || model.InchSize <= 0)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Please enter the valid input for Column Size and Inch Size." };

                if (string.IsNullOrEmpty(model.DatesBasedOffer))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Please add atleast DATE BASED OFFER" };

                if (model.InchSize > 20)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Inch size exceed the value of 20" };

                if (!model.UploadLater && (model.ImageContents == null || !model.ImageContents.Any()))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Uploaded file not found." };

                var modelEstimatedCosting = new PrivateDisplayGovtEstimatedCostingViewModel
                {
                    ColumnSize = model.ColumnSize,
                    InchSize = model.InchSize,
                    IsColor = model.IsColor,
                    Corporation = model.Corporation,
                    EditionPageId = model.EditionPageId
                };

                //fetch estimated total amount
                var estimatedCostingInfo = await FetchEstimatedTotalAmount(modelEstimatedCosting);

                if (estimatedCostingInfo.IsRateNotFound || estimatedCostingInfo.EstimatedCosting<=0)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Rate not found. Please try another edition page." };

                return new BaseResponse { IsSuccess = true, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        private List<ABPrintPrivateDisplayDetail> PopulatePrivateDisplayDetail(BookPrivateDisplayGovtAdViewModel model, DateTime currentDate)
        {
            var listing = new List<ABPrintPrivateDisplayDetail>();
            
            foreach (var date in model.DateBasedOfferList)
            {
                ABPrintPrivateDisplayDetail newABPrintPrivateDisplayDetail =
                    new ABPrintPrivateDisplayDetail
                    {
                        //ContentUrl = model.ContentUrl,
                        EditionId = model.EditionId,
                        EditionPageId = model.EditionPageId,
                        PublishDate = Convert.ToDateTime(date)
                    };

                listing.Add(newABPrintPrivateDisplayDetail);
            }
            return listing;
        }

        private async Task<ABPrintPrivateDisplay> PopulatePrivateDisplayInfo(BookPrivateDisplayGovtAdViewModel model, string bookingNo, DateTime currentDate)
        {
            decimal discountPercentage = 0;
            int offerDateId = 0;

            //fatch date offer
            var offerDate = await FatchDateOffer(model.DateBasedOfferList);
            if (offerDate != null && offerDate.OfferDateId > 0)
            {
                offerDateId = offerDate.OfferDateId;
                discountPercentage = offerDate.DiscountPercentage;
            }

            var modelEstimatedCosting = new PrivateDisplayGovtEstimatedCostingViewModel
            {
                ColumnSize = model.ColumnSize,
                InchSize = model.InchSize,
                IsColor = model.IsColor,
                Corporation = model.Corporation,
                EditionPageId = model.EditionPageId
            };

            //fetch estimated total amount
            var estimatedCostingInfo = await FetchEstimatedTotalAmount(modelEstimatedCosting);
            var estimatedTotal = estimatedCostingInfo.EstimatedCosting;
            model.EditionId = estimatedCostingInfo.EditionId;

            var newPrivateDisplay = new ABPrintPrivateDisplay
            {
                IsColor = model.IsColor,
                IsGovt = true,
                PrivateAdType= PrivateAdTypesConstants.Govt,
                UploadLater = model.UploadLater,
                BookingStep = BookingStepConstants.Booked,
                ColumnSize = (int)model.ColumnSize,
                InchSize = model.InchSize,
                BookingNo = bookingNo,
                CategoryId = model.CategoryId,
                BookedBy = CurrentLoginUser.UserId,
                BookDate = currentDate,
                AdStatus = 1,
                GrossTotal = estimatedTotal,
                DiscountPercent = discountPercentage,
                OfferDateId = offerDateId,
                Corporation = model.Corporation,
                Remarks = model.Remarks,
                InsideDhaka = model.InsideDhaka,
                IsCorrespondent = CurrentLoginUser.IsCorrespondentUser,
                //OfferEditionId = model. , //TODO: need to clarification
                UpazillaId = CurrentLoginUser.UpazillaId,
                AgencyId = model.AgencyId,               
                BrandId = model.BrandId, 
                ReferenceNo = model.ReferenceNo,
                CreatedBy = CurrentLoginUser.UserId,
                ModifiedBy = CurrentLoginUser.UserId
            };

            newPrivateDisplay.DiscountAmount = (newPrivateDisplay.GrossTotal * newPrivateDisplay.DiscountPercent) / 100;
            newPrivateDisplay.NetAmount = newPrivateDisplay.GrossTotal - newPrivateDisplay.DiscountAmount;

            decimal vat = 0;
            if (model.EditionId == EditionConstants.National) vat = 15;

            newPrivateDisplay.VATAmount = (newPrivateDisplay.NetAmount * vat) / 100;
            newPrivateDisplay.Commission = 0;

            //agency commission
            if (!model.Personal && model.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                if (agency != null && agency.AgencyId > 0)
                    newPrivateDisplay.Commission = agency.GDCommission;
            }
            else if (CurrentLoginUser.IsCorrespondentUser)
            {
                newPrivateDisplay.Commission = CurrentLoginUser.DefaultCommission;
            }

            return newPrivateDisplay;
        }

        private Advertiser PopulateAdvertiser(BookPrivateDisplayGovtAdViewModel model, DateTime currentDate)
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

        private async Task<PrivateDisplayGovtEstimatedCostingViewModel> FetchEstimatedTotalAmount(PrivateDisplayGovtEstimatedCostingViewModel model)
        {
            var editionPage = await editionPageService.GetDetailsById(model.EditionPageId);
            if (editionPage == null)
                return model;

            var editionId = CurrentLoginUser.EditionId ?? 0;
            model.EditionId = editionId;
            if (editionId == 0)
                return model;

            var ratesPrintGovtAd = await ratePrintGovtAdService.GetDefaultRatePrintGovtAd(editionId, editionPage.EditionPageNo);
            if (CurrentLoginUser.IsCRMUser && ratesPrintGovtAd == null)
            {
                model.IsRateNotFound = true;
                return model;
            }

            //if not found with default edition then try with national edition
            else if (CurrentLoginUser.IsCorrespondentUser && !model.Corporation &&
                (ratesPrintGovtAd==null || (model.IsColor && ratesPrintGovtAd.PerColumnInchColorRate <= 0) || (!model.IsColor && ratesPrintGovtAd.PerColumnInchBWRate <= 0)))
            {
                editionId = EditionConstants.National;
                model.EditionId = editionId;
                ratesPrintGovtAd = await ratePrintGovtAdService.GetDefaultRatePrintGovtAd(editionId, editionPage.EditionPageNo);

                if (ratesPrintGovtAd == null ||
                    (model.IsColor && ratesPrintGovtAd.PerColumnInchColorRate <= 0) || (!model.IsColor && ratesPrintGovtAd.PerColumnInchBWRate <= 0))
                {
                    model.IsRateNotFound = true;
                    return model;
                }

                model.IsNationalEditionRate = true;
            }

            //if not found with default edition then try with national edition
            else if (CurrentLoginUser.IsCorrespondentUser && model.Corporation &&
                (ratesPrintGovtAd == null || (model.IsColor && ratesPrintGovtAd.CorpColorRate <= 0) || (!model.IsColor && ratesPrintGovtAd.CorpBWRate <= 0)))
            {
                editionId = EditionConstants.National;
                model.EditionId = editionId;
                ratesPrintGovtAd = await ratePrintGovtAdService.GetDefaultRatePrintGovtAd(editionId, editionPage.EditionPageNo);

                if (ratesPrintGovtAd == null ||
                    ((model.IsColor && ratesPrintGovtAd.CorpColorRate <= 0) || (!model.IsColor && ratesPrintGovtAd.CorpBWRate <= 0)))
                {
                    model.IsRateNotFound = true;
                    return model;
                }

                model.IsNationalEditionRate = true;
            }

            //for color
            if (model.IsColor)
            {
                if (model.Corporation)
                {
                    model.EstimatedCosting = ratesPrintGovtAd.CorpColorRate * Convert.ToDecimal(model.ColumnSize) * Convert.ToDecimal(model.InchSize);
                    model.GovtAdRate = ratesPrintGovtAd.CorpColorRate;
                    return model;
                }
                else
                {
                    model.EstimatedCosting = ratesPrintGovtAd.PerColumnInchColorRate * Convert.ToDecimal(model.ColumnSize) * Convert.ToDecimal(model.InchSize);
                    model.GovtAdRate = ratesPrintGovtAd.PerColumnInchColorRate;
                    return model;
                }
            }

            //for black and white
            if (model.Corporation)
            {
                model.EstimatedCosting = ratesPrintGovtAd.CorpBWRate * Convert.ToDecimal(model.ColumnSize) * Convert.ToDecimal(model.InchSize);
                model.GovtAdRate = ratesPrintGovtAd.CorpBWRate;
                return model;
            }
            else
            {
                model.EstimatedCosting = ratesPrintGovtAd.PerColumnInchBWRate * Convert.ToDecimal(model.ColumnSize) * Convert.ToDecimal(model.InchSize);
                model.GovtAdRate = ratesPrintGovtAd.PerColumnInchBWRate;
                return model;
            }
        }

        //private async Task<List<PrivateDisplayMediaContent>> SaveAndGetAdImageContentUrl(BookPrivateDisplayGovtAdViewModel model, string bookingNo)
        //{
        //    var privateDisplayMediaContents = new List<PrivateDisplayMediaContent>();
                       
        //    string[] removeUploadedFiles = new string[999999];

        //    if (!string.IsNullOrWhiteSpace(model.RemoveUploadedFiles))
        //        removeUploadedFiles = model.RemoveUploadedFiles.Split(new[] { "@@AdPro@@" }, StringSplitOptions.None);
            
        //    int serialNo = 1;

        //    foreach (var adContent in model.ImageContents)
        //    {
        //        if (removeUploadedFiles.Any(f => f == adContent.FileName))
        //            continue;

        //        var fileName = $"{bookingNo}-ga-ad-content{Path.GetExtension(adContent.FileName)}";

        //        var request = new AzureFileUploadRequestModel
        //        {
        //            FileContent = adContent,
        //            Filename = fileName,
        //            BlobContainer = configuration["AzureService:Container:GovtAd"],
        //            BlobServiceClient = configuration["AzureService:BlobServiceClient"],
        //            BlobContentType = adContent.ContentType,
        //            IsIFormFile = true
        //        };

        //        // let's save to azure storage
        //        var responseAzureStorage = await azureServiceService.Upload(request);

        //        var dbSavePath = responseAzureStorage.AbsoluteUri;
        //        privateDisplayMediaContents.Add(new PrivateDisplayMediaContent
        //        {
        //            OriginalImageUrl = dbSavePath,
        //            CreatedDate = DateTime.Now
        //        });

        //        serialNo = serialNo + 1;
        //    }

        //    return privateDisplayMediaContents;
        //}

        private async Task<IEnumerable<SelectListItem>> GetEditionPageDropdown(int editionPageId = 0)
        {
            var defaultEditionId = CurrentLoginUser.EditionId ?? 0;
            var leadSource = await editionPageService.GetEditionPageListForDropdown(defaultEditionId);

            return leadSource.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = i.Value == editionPageId.ToString()
                }).ToList();
        }

        #endregion
    }
}
