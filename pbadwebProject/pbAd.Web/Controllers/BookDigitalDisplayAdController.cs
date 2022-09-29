using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABDigitalDisplays;
using pbAd.Data.Models;
using pbAd.Service.ABDigitalDisplays;
using pbAd.Service.Categories;
using pbAd.Service.Advertisers;
using pbAd.Service.DigitalAdUnitTypes;
using pbAd.Service.DigitalPagePositions;
using pbAd.Service.DigitalPages;
using pbAd.Service.RateDigitalDisplays;
using pbAd.Service.SubCategories;
using pbAd.Web.ViewModels.BookDigitalDisplayAds;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using pbAd.Core.Filters;
using pbAd.Service.Brands;
using pbAd.Service.Agencies;

using System.Globalization;

namespace pbAd.Web.Controllers
{
    public class BookDigitalDisplayAdController : AdminBaseController
    {
        #region Private Methods
        private readonly ICategoryService categoryService;
        private readonly IAdvertiserService advertiserService;
        private readonly ISubCategoryService subCategoryService;
        private readonly IABDigitalDisplayService abDigitalDisplayService;
        private readonly IRateDigitalDisplayService rateDigitalDisplayService;
        private readonly IDigitalAdUnitTypeService digitalAdUnitTypeService;
        private readonly IDigitalPageService digitalPageService;
        private readonly IDigitalPagePositionService digitalPagePositionService;
        private readonly IBrandService brandService;
        private readonly IAgencyService agencyService;
     
        Microsoft.Extensions.Configuration.IConfiguration configuration;

        #endregion

        #region Ctor

        public BookDigitalDisplayAdController(
                ICategoryService categoryService,
                IAdvertiserService advertiserService,
                ISubCategoryService subCategoryService,
                IBrandService brandService,
                IABDigitalDisplayService abDigitalDisplayService,
                IRateDigitalDisplayService rateDigitalDisplayService,
                IDigitalAdUnitTypeService digitalAdUnitTypeService,
                IDigitalPageService digitalPageService,
                IAgencyService agencyService,
              
                Microsoft.Extensions.Configuration.IConfiguration configuration,
                IDigitalPagePositionService digitalPagePositionService
            )
        {
            this.categoryService = categoryService;
            this.advertiserService = advertiserService;
            this.subCategoryService = subCategoryService;
            this.abDigitalDisplayService = abDigitalDisplayService;
            this.rateDigitalDisplayService = rateDigitalDisplayService;
            this.digitalAdUnitTypeService = digitalAdUnitTypeService;
            this.digitalPageService = digitalPageService;
            this.digitalPagePositionService = digitalPagePositionService;
            this.brandService = brandService;
           
            this.agencyService = agencyService;
            this.configuration = configuration;
        }

        #endregion

        #region Book Now
        public async Task<IActionResult> BookNow()
        {            
            var model = new BookDigitalDisplayAdViewModel
            {                
                DigitalAdUnitTypeDropdownList = GetDigitalAdUnitTypeDropdown(),
                DigitalPageDropdownList = await GetDigitalPageDropdown(),
                //DigitalPagePositionDropdownList = GetDigitalPagePositionDropdown(),
                AdQty=1
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BookNow(BookDigitalDisplayAdViewModel model)
        {
            ModelState.Remove("AgencyId"); ModelState.Remove("AdvertiserId");

            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
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
                    MobileNo = model.AdvertiserContactNumber
                };
                //Get details by mobile
                var existingAdvertiser = await advertiserService.GetAdvirtizerInfoByFilter(filter);

                //Populate Advertiser info
                if (existingAdvertiser != null)
                {
                    newAdvertiser = existingAdvertiser;
                    newAdvertiser.AdvertiserName = model.AdvertiserName;
                    newAdvertiser.AdvertiserMobileNo = model.AdvertiserContactNumber;
                }
                else newAdvertiser = PopulateAdvertiser(model, currentDate);

                var digitalDisplayMediaContents = new List<DigitalDisplayMediaContent>();
                if (!model.UploadLater)//let's save file & populate digital display media content
                    digitalDisplayMediaContents = null;

                //populate ab print classified text
                var newDigitalDisplay = await PopulateDigitalDisplayInfo(model, bookingNo, currentDate);

                //Populate ab print classified text detail
                var digitalDisplayDetailListing = PopulateABDigitalDisplayDetail(model, currentDate);

                newDigitalDisplay.TotalQty = digitalDisplayDetailListing.Sum(f => f.AdQty);

                var request = new BookDigitalDisplayAdProcessModel
                {
                    RequiredAdvertiser = true,
                    Advertiser = newAdvertiser,
                    ABDigitalDisplay = newDigitalDisplay,
                    ABDigitalDisplayDetailListing = digitalDisplayDetailListing,
                    DigitalDisplayMediaContents=digitalDisplayMediaContents
                };

                var response = await abDigitalDisplayService.Add(request);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/digitaldisplay/{response.Result.ABDigitalDisplayId}/payment/{response.Result.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }
        #endregion

        #region Booking Edit
        [Route("/bookdigitaldisplayad/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(int id, string bookingno)
        {
            //get digital display
            var bookingStep = BookingStepConstants.Booked;
            var digitalDisplay = await abDigitalDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);
            if (digitalDisplay == null)
                return Redirect("/bookdigitaldisplayad/booknow");

            //get digital display detail listings
            var digitalDisplayDetailListing = await abDigitalDisplayService.GetABDigitalDisplayDetailListing(id);

            if (!digitalDisplayDetailListing.Any())
                return Redirect("/bookdigitaldisplayad/booknow");

            var brand = await brandService.GetDetailsById(digitalDisplay.BrandId.Value);
            if (brand == null) brand = new Brand();

            var agency = await agencyService.GetDetailsById(digitalDisplay.AgencyId.Value);
            if (agency == null) agency = new Agency();

            var advertiser = await advertiserService.GetDetailsById(digitalDisplay.AdvertiserId);
            if (advertiser == null) advertiser = new Advertiser();

            var model = new BookDigitalDisplayAdViewModel
            {
                BrandAutoComplete = brand.BrandName,
                BrandId = brand.BrandId,
                AgencyAutoComplete = agency.AgencyName,
                AgencyId = agency.AgencyId,
                AdvertiserName = advertiser.AdvertiserName,
                AdvertiserId = advertiser.AdvertiserId,
                AdvertiserContactNumber = advertiser.AdvertiserMobileNo,
                AdQty=1,
                
                UploadLater = digitalDisplay.UploadLater,
                
                EstimatedTotal = digitalDisplay.GrossTotal,
                CategoryId = digitalDisplay.CategoryId,
                Remarks = digitalDisplay.Remarks,
                BookingNo = digitalDisplay.BookingNo,
                ABDigitalDisplayId = digitalDisplay.ABDigitalDisplayId,

                DigitalAdUnitTypeDropdownList = GetDigitalAdUnitTypeDropdown(),
                DigitalPageDropdownList = await GetDigitalPageDropdown(),
                //DigitalPagePositionDropdownList = GetDigitalPagePositionDropdown(),

                DigitalDisplayDetailListing= digitalDisplayDetailListing,
            };

            if (!model.UploadLater)
            {
                //get media content listings
                var displayMediaContentListing = await abDigitalDisplayService.GetDigitalDisplayMediaContentListing(id);
                model.MediaContents = displayMediaContentListing;
                var firstMediContent = displayMediaContentListing.FirstOrDefault();
                if (firstMediContent != null) 
                {
                    model.ScreenType = firstMediContent.ScreenType;
                    model.CheckScreenTypeDesktop = model.ScreenType == ScreenTypeConstants.Desktop ? "checked" : "";
                    model.CheckScreenTypeMobile = model.ScreenType == ScreenTypeConstants.Mobile ? "checked" : "";
                }
            }

            return View(model);
        }

        [HttpPost]
        [Route("/bookdigitaldisplayad/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(BookDigitalDisplayAdViewModel model)
        {
            ModelState.Remove("AgencyId"); ModelState.Remove("AdvertiserId");

            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                var responseValidateForm = await ValidateEditForm(model);
                if (!responseValidateForm.IsSuccess)
                    return Json(new { status = false, message = responseValidateForm.Message, type = ActionResultTypeConstants.Message });

                var randomNo = CoreHelper.GenerateRandomToken(8);
                var currentDate = DateTime.Now;                

                //Populate Advertiser info
                var newAdvertiser = new Advertiser();
                var filter = new AdvertiserSearchFilter
                {
                    AdvertiserId = model.AdvertiserId,
                    MobileNo = model.AdvertiserContactNumber
                };
                //Get details by mobile
                var existingAdvertiser = await advertiserService.GetAdvirtizerInfoByFilter(filter);

                //Populate Advertiser info
                if (existingAdvertiser != null)
                {
                    newAdvertiser = existingAdvertiser;
                    newAdvertiser.AdvertiserName = model.AdvertiserName;
                    newAdvertiser.AdvertiserMobileNo = model.AdvertiserContactNumber;
                }
                else newAdvertiser = PopulateAdvertiser(model, currentDate);

                var digitalDisplayMediaContents = new List<DigitalDisplayMediaContent>();
                if (!model.UploadLater && model.ImageContents != null)//let's save file & populate digital display media content
                    digitalDisplayMediaContents = null;

                //populate ab print classified text
                var newDigitalDisplay = await PopulateDigitalDisplayInfo(model, model.BookingNo, currentDate);

                //Populate ab print classified text detail
                var digitalDisplayDetailListing = PopulateABDigitalDisplayDetail(model, currentDate);

                newDigitalDisplay.TotalQty = digitalDisplayDetailListing.Sum(f => f.AdQty);

                var request = new BookDigitalDisplayAdProcessModel
                {
                    RequiredAdvertiser = true,
                    Advertiser = newAdvertiser,
                    ScreenType = model.ScreenType,
                    ABDigitalDisplay = newDigitalDisplay,
                    ABDigitalDisplayDetailListing = digitalDisplayDetailListing,
                    RemoveExistingFiles = model.RemoveExistingFiles,
                    DigitalDisplayMediaContents = digitalDisplayMediaContents
                };

                var response = await abDigitalDisplayService.EditDigitalDisplayBook(request);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/digitaldisplay/{response.Result.ABDigitalDisplayId}/payment/{response.Result.BookingNo}";

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

        public async Task<JsonResult> GetBrandWiseAdvirtiser(int brandId)
        {
            var filter = new BrandSearchFilter
            {
                BrandId = brandId
            };

            var brandRelation = await brandService.GetBrandWiseAdvirtiser(filter);

            return Json(brandRelation);
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

        public async Task<JsonResult> GetRateForDigitalDisplay(int digitalAdUnitTypeId, int digitalPageId, int digitalPagePositionId)
        {
            try
            {
                var rateDigitalDisplay = await rateDigitalDisplayService
                    .GetByUnitTypePageAndPosition(digitalAdUnitTypeId, digitalPageId, digitalPagePositionId);

                return Json(rateDigitalDisplay);
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDigitalDisplayListing(int digitalDisplayId)
        {
            try
            {
                //get digital display listings
                var digitalDisplayListings = await abDigitalDisplayService.GetABDigitalDisplayDetailListing(digitalDisplayId);

                foreach (var item in digitalDisplayListings)
                {
                    item.PublishDateStartInText = item.PublishDateStart.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    item.PublishTimeStartInText = item.PublishDateStart.ToString("hh:mm tt", CultureInfo.InvariantCulture);

                    item.PublishDateEndInText = item.PublishDateEnd.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                    item.PublishTimeEndInText = item.PublishDateEnd.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                }

                return Json(new { Data = digitalDisplayListings });
            }
            catch (Exception e)
            {
                return Json(new { Data = "" });
            }
        }

        public async Task<JsonResult> GetPagePositionByPage(int pageId)
        {
            var defaultSelectionList = new List<SelectListItem> { new SelectListItem { Text = "Select Page Position", Value = "" } };

            //if page wise page position found found then populate ddl
            var pagePositions = await digitalPagePositionService.GetDigitalPagePositionListForDropdown(pageId);
            if (pagePositions.Any())
            {
                var newPagePositions =
                        from c in pagePositions
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

                defaultSelectionList.AddRange(newPagePositions);                
            }

            return Json(defaultSelectionList);
        }

        #endregion

        #region Private Methods

        //private async Task<List<DigitalDisplayMediaContent>> SaveAndGetAdImageContentUrl(BookDigitalDisplayAdViewModel model, string bookingNo)
        //{
        //    var digitalDisplayMediaContents = new List<DigitalDisplayMediaContent>();

        //    string[] removeUploadedFiles = new string[999999];

        //    if (!string.IsNullOrWhiteSpace(model.RemoveUploadedFiles))
        //        removeUploadedFiles = model.RemoveUploadedFiles.Split(new[] { "@@AdPro@@" }, StringSplitOptions.None);
        //    int serialNo = 1;

        //    foreach (var adContent in model.ImageContents)
        //    {
        //        if (removeUploadedFiles.Any(f => f == adContent.FileName))
        //            continue;

        //        var fileName = $"{bookingNo}-pd-ad-content{Path.GetExtension(adContent.FileName)}";

        //        var request = new AzureFileUploadRequestModel
        //        {
        //            FileContent = adContent,
        //            Filename = fileName,
        //            BlobContainer = configuration["AzureService:Container:DigitalDisplay"],
        //            BlobServiceClient = configuration["AzureService:BlobServiceClient"],
        //            BlobContentType = adContent.ContentType,
        //            IsIFormFile = true
        //        };

        //        // let's save to azure storage
        //        var responseAzureStorage = await azureServiceService.Upload(request);

        //        var dbSavePath = responseAzureStorage.AbsoluteUri;

        //        digitalDisplayMediaContents.Add(new DigitalDisplayMediaContent
        //        {
        //            ScreenType=model.ScreenType,
        //            OriginalImageUrl = dbSavePath,
        //            CreatedDate = DateTime.Now
        //        });

        //        serialNo = serialNo + 1;
        //    }

        //    return digitalDisplayMediaContents;
        //}

        private async Task<BaseResponse> ValidateEditForm(BookDigitalDisplayAdViewModel model)
        {
            try
            {
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

                if (!model.UploadLater && !model.IsFoundExistingFiles && (model.ImageContents == null || !model.ImageContents.Any()))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Uploaded file not found." };

                var modelEstimatedCosting = new DigitalDisplayEstimatedCostingViewModel
                {
                    DigitalDisplayDetailList = model.DigitalDisplayDetailList
                };

                //fetch estimated total amount
                var estimatedCostingInfo = await FetchEstimatedTotalAmount(modelEstimatedCosting);

                if (estimatedCostingInfo.EstimatedCosting <= 0)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Rate not found. Please try another edition page." };

                return new BaseResponse { IsSuccess = true, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        private async Task<BaseResponse> ValidateForm(BookDigitalDisplayAdViewModel model)
        {
            try
            {
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

                if (!model.UploadLater && (model.ImageContents == null || !model.ImageContents.Any()))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Uploaded file not found." };

                var modelEstimatedCosting = new DigitalDisplayEstimatedCostingViewModel
                {
                    DigitalDisplayDetailList = model.DigitalDisplayDetailList
                };

                //fetch estimated total amount
                var estimatedCostingInfo = await FetchEstimatedTotalAmount(modelEstimatedCosting);

                if (estimatedCostingInfo.EstimatedCosting <= 0)
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Rate not found. Please try another edition page." };

                return new BaseResponse { IsSuccess = true, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
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

        private List<ABDigitalDisplayDetail> PopulateABDigitalDisplayDetail(BookDigitalDisplayAdViewModel model, DateTime currentDate)
        {
            var digitalDisplayDetailList = new List<ABDigitalDisplayDetail>();
           
            foreach (var item in model.DigitalDisplayDetailList)
            {
                var publishDateStart = new DateTime();
                var publishDateEnd = new DateTime();

                try
                {
                    publishDateStart = DateTime.Parse(item.PublishDateStart + " " + model.PublishTimeStart);
                    publishDateEnd = DateTime.Parse(item.PublishDateEnd + " " + model.PublishTimeEnd);
                }
                catch
                {
                    continue;
                }

                var newABDigitalDisplayDetail = new ABDigitalDisplayDetail
                {
                    DigitalAdUnitTypeId= item.DigitalAdUnitTypeId,
                    DigitalPageId = item.DigitalPageId,
                    DigitalPagePositionId = item.DigitalPagePositionId,
                    AdQty = item.AdQty,
                    PerUnitRate = item.PerUnitRate,
                    PublishDateStart = publishDateStart,
                    PublishDateEnd = publishDateEnd,
                };

                digitalDisplayDetailList.Add(newABDigitalDisplayDetail);
            }

            return digitalDisplayDetailList;
        }

        private async Task<ABDigitalDisplay> PopulateDigitalDisplayInfo(BookDigitalDisplayAdViewModel model, string guid, DateTime currentDate)
        {
            var modelEstimatedCosting = new DigitalDisplayEstimatedCostingViewModel
            {
                DigitalDisplayDetailList = model.DigitalDisplayDetailList
            };

            //fetch estimated total amount
            var estimatedCostingInfo = await FetchEstimatedTotalAmount(modelEstimatedCosting);
            var estimatedTotal = estimatedCostingInfo.EstimatedCosting;

            var newDigitalDisplay = new ABDigitalDisplay
            {
                ABDigitalDisplayId=model.ABDigitalDisplayId,
                BookingNo = guid,
                BookingStep = BookingStepConstants.Booked,
                CategoryId = model.CategoryId,
                SubCategoryId = model.SubCategoryId,
                BookedBy = CurrentLoginUser.UserId,
                UploadLater=model.UploadLater,
                BookDate = currentDate,
                AdStatus = 1,
                AdvertiserId = model.AdvertiserId,
                ActualReceived = 0,
                GrossTotal = estimatedTotal,
                DiscountPercent = 0,
                DiscountAmount = 0,
                OfferDateId = 0 , //TODO: need to clarification
                OfferEditionId = 0, //TODO: need to clarification
                UpazillaId = CurrentLoginUser.UpazillaId,
                AgencyId = model.AgencyId, //TODO: need to clarification
                BrandId = model.BrandId, //TODO: need to clarification
                Remarks=model.Remarks,
                IsCorrespondent = CurrentLoginUser.IsCorrespondentUser,
                CreatedBy = CurrentLoginUser.UserId,
                ModifiedBy = CurrentLoginUser.UserId
            };

            newDigitalDisplay.DiscountAmount = (newDigitalDisplay.GrossTotal * newDigitalDisplay.DiscountPercent) / 100;
            newDigitalDisplay.NetAmount = newDigitalDisplay.GrossTotal - newDigitalDisplay.DiscountAmount;

            decimal vat = 0;
            if (model.EditionId == EditionConstants.National) vat = 15;

            newDigitalDisplay.VATAmount = (newDigitalDisplay.NetAmount * vat) / 100;
            newDigitalDisplay.Commission = 0;

            //agency commission
            if (model.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                if (agency != null && agency.AgencyId > 0)
                    newDigitalDisplay.Commission = agency.DDCommission;
            }
            else if (CurrentLoginUser.IsCorrespondentUser)
            {
                newDigitalDisplay.Commission = CurrentLoginUser.DefaultCommission;
            }

            return newDigitalDisplay;
        }

        private Advertiser PopulateAdvertiser(BookDigitalDisplayAdViewModel model, DateTime currentDate)
        {
            var advertiserType = CurrentLoginUser.IsCRMUser ? 1 : 3;
            return new Advertiser
            {
                AdvertiserName = model.AdvertiserName,
                AdvertiserType = advertiserType,
                AdvertiserEmail = "N/A",
                AdvertiserMobileNo = model.AdvertiserContactNumber,
                CreatedBy = CurrentLoginUser.UserId,
                CreatedDate = currentDate
            };
        }


        private IEnumerable<SelectListItem> GetDigitalAdUnitTypeDropdown(int selected = 0)
        {
            var leadSource = digitalAdUnitTypeService.GetDigitalAdUnitTypeListForDropdown();

            return leadSource.Result.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = selected == Convert.ToInt32(i.Value)
                }).ToList();
        }

        private async Task<IEnumerable<SelectListItem>> GetDigitalPageDropdown(int selected = 0)
        {
            var leadSource = await digitalPageService.GetDigitalPageListForDropdown();

            return leadSource.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = selected == Convert.ToInt32(i.Value)
                }).ToList();
        }

        /*
        private IEnumerable<SelectListItem> GetDigitalPagePositionDropdown(int selected = 0)
        {
            var leadSource = digitalPagePositionService.GetDigitalPagePositionListForDropdown();

            return leadSource.Result.Select(
                i => new SelectListItem
                {
                    Value = i.Value,
                    Text = i.Text,
                    Selected = selected == Convert.ToInt32(i.Value)
                }).ToList();
        }
        */


        #endregion
    }
}
