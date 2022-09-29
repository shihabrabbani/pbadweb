#region Usings

using CoreHtmlToImage;
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.BookClassifiedDisplayAds;
using pbAd.Data.Models;
using pbAd.Service.ABPrintClassifiedDisplays;
using pbAd.Service.Advertisers;
using pbAd.Service.Agencies;

using pbAd.Service.Categories;
using pbAd.Service.EditionPages;
using pbAd.Service.OfferDates;
using pbAd.Service.RatePrintClassifiedDisplays;
using pbAd.Service.SubCategories;
using pbAd.Web.Infrastructure.Helpers;
using pbAd.Web.ViewModels.BookClasssifiedDisplayAds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace pbAd.Web.Controllers
{
    public class BookClasssifiedDisplayAdController : AdminBaseController
    {
        #region Private Methods
        private readonly ICategoryService categoryService;
        private readonly IAdvertiserService consumerService;
        private readonly ISubCategoryService subCategoryService;
        private readonly IABPrintClassifiedDisplayService abPrintClassifiedDisplayService;
        private readonly IRatePrintClassifiedDisplayService ratePrintClassifiedDisplayService;
        private readonly IAgencyService agencyService;
        private readonly IEditionPageService editionPageService;
        private readonly IAdvertiserService advertiserService;
        private readonly IOfferDateService offerDateService;
  
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        #endregion

        #region Ctor

        public BookClasssifiedDisplayAdController(
             ICategoryService categoryService,
             IAdvertiserService consumerService,
             ISubCategoryService subCategoryService,
             IABPrintClassifiedDisplayService abPrintClassifiedDisplayService,
             IRatePrintClassifiedDisplayService ratePrintClassifiedDisplayService,
             IAgencyService agencyService,
             IAdvertiserService advertiserService,
             IEditionPageService editionPageService,
             IOfferDateService offerDateService,
            
             Microsoft.Extensions.Configuration.IConfiguration configuration
            )
        {
            this.categoryService = categoryService;
            this.consumerService = consumerService;
            this.subCategoryService = subCategoryService;
            this.abPrintClassifiedDisplayService = abPrintClassifiedDisplayService;
            this.ratePrintClassifiedDisplayService = ratePrintClassifiedDisplayService;
            this.agencyService = agencyService;
            this.editionPageService = editionPageService;
            this.advertiserService = advertiserService;
            this.offerDateService = offerDateService;

            this.configuration = configuration;
        }

        #endregion

        #region Book Now

        public async Task<IActionResult> BookNow()
        {
            var editionId = CurrentLoginUser.EditionId ?? 0;
            var rateClassifiedDisplay = await ratePrintClassifiedDisplayService.GetDefaultRatePrintClassifiedDisplay(editionId);

            if (rateClassifiedDisplay == null || rateClassifiedDisplay.AutoId <= 0)
                return Redirect("/dashboard");

            decimal defaultInchRate = rateClassifiedDisplay.Rate1ColInch;

            //store rates into session
            HttpContext.Session.SetObject(SessionKeyConstants.RatePrintClassifiedDisplay, rateClassifiedDisplay);

            var model = new BookClasssifiedDisplayAdViewModel
            {
                CategoryDropdownList = await GetCategoryDropdown(),
                RatePrintClassifiedDisplay = rateClassifiedDisplay,
                DefaultInchRate = defaultInchRate
            };

            model.MaxTitleWords = model.RatePrintClassifiedDisplay.MaxTitleWords;
            model.MaxContentWords = model.RatePrintClassifiedDisplay.MaxContentWords;
            model.PerColumnInchRate = model.RatePrintClassifiedDisplay.PerColumnInch;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BookNow(BookClasssifiedDisplayAdViewModel model)
        {
            if (model.CompleteMatter)
            {
                ModelState.Remove("Title");
                ModelState.Remove("AdContent");
            }
            ModelState.Remove("AgencyId");
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                //validate form
                var reponseValidation = await ValidateForm(model);
                if (!reponseValidation.IsSuccess)
                    return Json(new { status = false, message = reponseValidation.Message, type = ActionResultTypeConstants.Message });

                var bookingNo = CoreHelper.GenerateRandomToken(8);
                var currentDate = DateTime.Now;
                model.EditionId = CurrentLoginUser.EditionId ?? 0;

                var newAdvertiser = new Advertiser();
                if (model.Personal || CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
                {
                    model.RequiredAdvertiser = true;

                    //if ((model.AdvertiserContactNumber).Length != 11)
                    //    return Json(new { status = false, message = "Warning, Contact Number Must be 11 digits", type = ActionResultTypeConstants.Message });

                    ////Get details by mobile
                    //var existingAdvertiser = await consumerService.GetDetailsByMobile(model.AdvertiserContactNumber);
                    ////Populate Advertiser info

                    //if (existingAdvertiser != null) { newAdvertiser = existingAdvertiser; newAdvertiser.AdvertiserName = model.AdvertiserName; }
                    //else 
                        
                        newAdvertiser = PopulateAdvertiser(model, currentDate);
                }

                //if (model.ImageContent != null && model.ImageContent.Length > 0)
                //{
                //    //let's save & populate original image content url
                //    var responseOrginalImage = await SaveAndGetOrginalImageContentUrl(model, bookingNo);
                //    if (!responseOrginalImage.IsSuccess)
                //        return Json(new { status = false, message = responseOrginalImage.Message, type = ActionResultTypeConstants.Message });

                //    model.OriginalImageUrl = responseOrginalImage.Result;
                //}

                //if (!model.CompleteMatter)
                //{
                //    //let's save & populate final image content url
                //    var responseFinalImage = await SaveAndGetFinalImageContentUrl(model, bookingNo);
                //    if (!responseFinalImage.IsSuccess)
                //        return Json(new { status = false, message = responseFinalImage.Message, type = ActionResultTypeConstants.Message });

                //    model.FinalImageUrl = responseFinalImage.Result;
                //}

                if (model.CompleteMatter)
                    model.FinalImageUrl = model.OriginalImageUrl;

                model.DateBasedOfferList = model.DatesBasedOffer.Split(',');

                //populate ab print classified text
                var newClassifiedDisplay = await PopulateClassifiedDisplayInfo(model, bookingNo, currentDate);

                //Populate ab print classified text detail
                var abPrintClassifiedDisplayDetailListing = await PopulateABPrintClassifiedDisplayDetail(model, currentDate);

                var request = new BookClassifiedDisplayAdProcessModel
                {
                    RequiredAdvertiser = model.RequiredAdvertiser,
                    Advertiser = newAdvertiser,
                    ABPrintClassifiedDisplay = newClassifiedDisplay,
                    ABPrintClassifiedDisplayDetailListing = abPrintClassifiedDisplayDetailListing
                };

                var response = await abPrintClassifiedDisplayService.BookClassifiedDisplayAd(request);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/classifieddisplay/{response.Result.ABPrintClassifiedDisplayId}/payment/{response.Result.BookingNo}";

                return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Booking Edit
        [Route("/bookclasssifieddisplayad/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(int id, string bookingno)
        {
            var bookingStep = BookingStepConstants.Booked;
            var classifiedDisplay = await abPrintClassifiedDisplayService.GetDetails(id, bookingno, bookingStep, CurrentLoginUser.UserId);
            if (classifiedDisplay == null)
                return Redirect("/bookclasssifieddisplayad/booknow");

            var classifiedDisplayDetailListing = await abPrintClassifiedDisplayService.GetABPrintClassifiedDisplayDetailListing(id);

            if (!classifiedDisplayDetailListing.Any())
                return Redirect("/bookclasssifieddisplayad/booknow");

            var editionId = CurrentLoginUser.EditionId ?? 0;
            var rateClassifiedDisplay = await ratePrintClassifiedDisplayService.GetDefaultRatePrintClassifiedDisplay(editionId);

            if (rateClassifiedDisplay == null || rateClassifiedDisplay.AutoId <= 0)
                return Redirect("/dashboard");

            var classifiedDisplayDetail = classifiedDisplayDetailListing.FirstOrDefault();

            decimal defaultInchRate = rateClassifiedDisplay.Rate1ColInch;

            //store rates into session
            HttpContext.Session.SetObject(SessionKeyConstants.RatePrintClassifiedDisplay, rateClassifiedDisplay);

            var agencyInfo = new Agency();
            var agency = await agencyService.GetDetailsById(classifiedDisplay.AgencyId.Value);
            if (agency != null)
                agencyInfo = agency;

            var advertiserInfo = new Advertiser();
            var advertiser = await advertiserService.GetDetailsById(classifiedDisplay.AdvertiserId);
            if (advertiser != null)
                advertiserInfo = advertiser;

            var basedOfferDates = classifiedDisplayDetailListing.Select(s => s.PublishDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)).ToList();
            var basedOfferDatesInString = String.Join(", ", basedOfferDates);

            var model = new BookClasssifiedDisplayAdViewModel
            {
                ABPrintClassifiedDisplayId = classifiedDisplay.ABPrintClassifiedDisplayId,
                BookingNo = classifiedDisplay.BookingNo,
                CategoryId = classifiedDisplay.CategoryId,
                SubCategoryId = classifiedDisplay.SubCategoryId,
                CategoryDropdownList = await GetCategoryDropdown(classifiedDisplay.CategoryId),
                SubCategoryDropdownList = await GetSubCategoryDropdown(classifiedDisplay.CategoryId, classifiedDisplay.SubCategoryId),

                AgencyInfo = agencyInfo,
                AgencyId = agencyInfo.AgencyId,
                AdvertiserInfo = advertiserInfo,

                AgencyAutoComplete = agencyInfo.AgencyName,
                AdvertiserName = advertiserInfo.AdvertiserName,
                AdvertiserContactNumber = advertiserInfo.AdvertiserMobileNo,
                CompleteMatter = classifiedDisplay.CompleteMatter,
                AdColumnInch = classifiedDisplay.AdColumnInch,
                EstimatedTotal = classifiedDisplay.GrossTotal,

                RatePrintClassifiedDisplay = rateClassifiedDisplay,
                DefaultInchRate = defaultInchRate,
                DatesBasedOffer = basedOfferDatesInString,
                OriginalImageUrl = classifiedDisplayDetail.OriginalImageUrl,
                FinalImageUrl = classifiedDisplayDetail.FinalImageUrl,
                AdContent = classifiedDisplayDetail.AdContent,
                Title = classifiedDisplayDetail.Title,
                Remarks = classifiedDisplay.Remarks
            };

            model.MaxTitleWords = model.RatePrintClassifiedDisplay.MaxTitleWords;
            model.MaxContentWords = model.RatePrintClassifiedDisplay.MaxContentWords;
            model.PerColumnInchRate = model.RatePrintClassifiedDisplay.PerColumnInch;

            return View(model);
        }

        [HttpPost]
        [Route("/bookclasssifieddisplayad/edit/{id}/bookingno/{bookingno}")]
        public async Task<IActionResult> BookingEdit(BookClasssifiedDisplayAdViewModel model)
        {
            if (model.CompleteMatter)
            {
                ModelState.Remove("Title");
                ModelState.Remove("AdContent");
            }
            ModelState.Remove("AgencyId");

            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                //validate form
                var reponseValidation = await ValidateEditForm(model);
                if (!reponseValidation.IsSuccess)
                    return Json(new { status = false, message = reponseValidation.Message, type = ActionResultTypeConstants.Message });

                var randomNo = CoreHelper.GenerateRandomToken(8);
                var currentDate = DateTime.Now;
                model.EditionId = CurrentLoginUser.EditionId ?? 0;

                var newAdvertiser = new Advertiser();
                if (model.Personal || CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
                {
                    model.RequiredAdvertiser = true;

                    //if ((model.AdvertiserContactNumber).Length != 11)
                    //    return Json(new { status = false, message = "Warning, Contact Number Must be 11 digits", type = ActionResultTypeConstants.Message });

                    ////Get details by mobile
                    //var existingAdvertiser = await consumerService.GetDetailsByMobile(model.AdvertiserContactNumber);
                    ////Populate Advertiser info

                    //if (existingAdvertiser != null) { newAdvertiser = existingAdvertiser; newAdvertiser.AdvertiserName = model.AdvertiserName; }
                    //else 
                        
                        newAdvertiser = PopulateAdvertiser(model, currentDate);
                }

                //if (model.ImageContent != null && model.ImageContent.Length > 0)
                //{
                //    //let's save & populate original image content url
                //    var responseOrginalImage = await SaveAndGetOrginalImageContentUrl(model, randomNo);
                //    if (!responseOrginalImage.IsSuccess)
                //        return Json(new { status = false, message = responseOrginalImage.Message, type = ActionResultTypeConstants.Message });

                //    model.OriginalImageUrl = responseOrginalImage.Result;
                //}

                //if (!model.CompleteMatter && string.IsNullOrWhiteSpace(model.OriginalImageUrl) && string.IsNullOrWhiteSpace(model.FinalImageUrl))
                //{
                //    //let's save & populate final image content url
                //    var responseFinalImage = await SaveAndGetFinalImageContentUrl(model, randomNo);
                //    if (!responseFinalImage.IsSuccess)
                //        return Json(new { status = false, message = responseFinalImage.Message, type = ActionResultTypeConstants.Message });

                //    model.FinalImageUrl = responseFinalImage.Result;
                //}

                if (model.CompleteMatter)
                    model.FinalImageUrl = model.OriginalImageUrl;

                model.DateBasedOfferList = model.DatesBasedOffer.Split(',');

                //populate ab print classified text
                var newClassifiedDisplay = await PopulateClassifiedDisplayInfo(model, model.BookingNo, currentDate);

                //Populate ab print classified text detail
                var abPrintClassifiedDisplayDetailListing = await PopulateABPrintClassifiedDisplayDetail(model, currentDate);

                var request = new BookClassifiedDisplayAdProcessModel
                {
                    RequiredAdvertiser = model.RequiredAdvertiser,
                    Advertiser = newAdvertiser,
                    ABPrintClassifiedDisplay = newClassifiedDisplay,
                    ABPrintClassifiedDisplayDetailListing = abPrintClassifiedDisplayDetailListing
                };

                var response = await abPrintClassifiedDisplayService.EditBookedClassifiedDisplayAd(request);

                if (!response.IsSuccess)
                    return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

                var returnUrl = $"/checkout/classifieddisplay/{model.ABPrintClassifiedDisplayId}/payment/{model.BookingNo}";

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

        [HttpPost]
        public JsonResult GetAdColumnInchRate(decimal adColumnInch)
        {
            try
            {
                //get rates from session
                var rateClassifiedDisplay = HttpContext.Session.GetObject<RatePrintClassifiedDisplay>(SessionKeyConstants.RatePrintClassifiedDisplay);

                if (rateClassifiedDisplay == null)
                    return Json(new { status = false, message = "Rate not found. Please try again!", type = ActionResultTypeConstants.Message });

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

                    default:
                        adColumnInchRate = 0;
                        break;
                }

                return Json(new { status = true, adColumnInchRate = (int)(Math.Round(adColumnInchRate, 0, MidpointRounding.AwayFromZero)), type = ActionResultTypeConstants.Message });
            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "Warning, Error on getting rate. Please contact with support.", type = ActionResultTypeConstants.Message });
            }
        }

        #endregion

        #region Private Methods

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

        //private async Task<Response<string>> SaveAndGetOrginalImageContentUrl(BookClasssifiedDisplayAdViewModel model, string bookingNo)
        //{
        //    var response = new Response<string>();

        //    try
        //    {
        //        var fileNameForOriginal = $"{bookingNo}-original-add-content{Path.GetExtension(model.ImageContent.FileName)}";

        //        var request = new AzureFileUploadRequestModel
        //        {
        //            FileContent = model.ImageContent,
        //            Filename = fileNameForOriginal,
        //            BlobContainer = configuration["AzureService:Container:ClassifiedDisplay"],
        //            BlobServiceClient = configuration["AzureService:BlobServiceClient"],
        //            BlobContentType = model.ImageContent.ContentType,
        //            IsIFormFile = true
        //        };

        //        var responseAzureStorage = await azureServiceService.Upload(request);

        //        var dbSavePath = responseAzureStorage.AbsoluteUri; 
        //        response.Result = dbSavePath;
        //        response.IsSuccess = true;

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = "Error on Saving file";
        //        response.IsSuccess = false;

        //        return response;
        //    }
        //}
        private async Task<Response<decimal>> FetchEstimatedTotalAmount(decimal adColumnInch)
        {
            var reponse = new Response<decimal>();

            var rateClassifiedDisplay = new RatePrintClassifiedDisplay();
            decimal estimatedCosting = 0;

            //get default rates
            rateClassifiedDisplay = HttpContext.Session.GetObject<RatePrintClassifiedDisplay>(SessionKeyConstants.RatePrintClassifiedDisplay);
            if (rateClassifiedDisplay == null || rateClassifiedDisplay.AutoId <= 0)
            {
                var editionId = CurrentLoginUser.EditionId ?? 0;
                rateClassifiedDisplay = await ratePrintClassifiedDisplayService.GetDefaultRatePrintClassifiedDisplay(editionId);

                if (rateClassifiedDisplay == null)
                {
                    reponse.Result = 0;
                    reponse.Message = "Rate not found. Please contact with support team!";
                    return reponse;
                }
            }

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

                default:
                    adColumnInchRate = 0;
                    break;
            }

            estimatedCosting = Convert.ToDecimal(adColumnInchRate);
            //estimatedCosting = Convert.ToDecimal(adColumnInch) * Convert.ToDecimal(adColumnInchRate);

            reponse.Result = estimatedCosting;
            reponse.IsSuccess = true;
            return reponse;
        }

        private async Task<Response<ClasssifiedDisplayEstimatedCostingViewModel>> FetchFinalImage(ClasssifiedDisplayEstimatedCostingViewModel model)
        {
            var reponse = new Response<ClasssifiedDisplayEstimatedCostingViewModel>();

            //Get html ad content to bytes
            byte[] htmlToImagebytes = await GetHtmlAdContentToBytes(model);

            int width = (int)(ClassifiedDisplayAdWidthConstants.Inch_1_5 * 96);
            int height = (int)(model.AdColumnInch * 96);

            //Extract New Image
            Image finalImage = ExtractNewImage(htmlToImagebytes, width, height);

            model.FinalImage = finalImage;

            reponse.Result = model;
            reponse.IsSuccess = true;
            return reponse;
        }

        private Image ExtractNewImage(byte[] htmlToImagebytes, int width, int height)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(htmlToImagebytes));

            var bitmap = new Bitmap(width, height);

            int imageHeight = height;//pixel
            int imageWidth = width; //pixel

            using (var graphic = Graphics.FromImage(bitmap))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, imageWidth, imageHeight);
            }

            System.Drawing.Image finalImage = bitmap;

            return finalImage;
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(t => t.MimeType == mimeType);
        }

        //private async Task<Response<string>> SaveAndGetFinalImageContentUrl(BookClasssifiedDisplayAdViewModel model, string bookingNo)
        //{
        //    var response = new Response<string>();

        //    var displayEstimatedCostingModel = new ClasssifiedDisplayEstimatedCostingViewModel
        //    {
        //        ABPrintClassifiedDisplayId = model.ABPrintClassifiedDisplayId,
        //        Title = model.Title,
        //        AdContent = model.AdContent,
        //        ImageContent = model.ImageContent,
        //        AdColumnInch = model.AdColumnInch,
        //    };

        //    //fetch final image
        //    var responseImageResonse = await FetchFinalImage(displayEstimatedCostingModel);
        //    response.IsSuccess = responseImageResonse.IsSuccess;
        //    response.Message = responseImageResonse.Message;

        //    if (!responseImageResonse.IsSuccess)
        //        return response;

        //    var qualityParam = new EncoderParameter(Encoder.Quality, 100L);
        //    var jpegCodec = GetEncoderInfo("image/jpeg");

        //    var encoderParams = new EncoderParameters(1);
        //    encoderParams.Param[0] = qualityParam;

        //    var fileNameForFinal = $"{bookingNo}-final-ad-content.jpg";

        //    var request = new AzureFileUploadRequestModel
        //    {                
        //        ImageFile= responseImageResonse.Result.FinalImage,
        //        Filename = fileNameForFinal,
        //        BlobContainer= configuration["AzureService:Container:ClassifiedDisplay"],                
        //        BlobServiceClient = configuration["AzureService:BlobServiceClient"],
        //        BlobContentType = "image/jpg",
        //        IsIFormFile=false
        //    };

        //    var responseAzureStorage = await azureServiceService.Upload(request);

        //    var dbSavePath = responseAzureStorage.AbsoluteUri;
        //    response.Result = dbSavePath;

        //    return response;
        //}

        private async Task<byte[]> GetHtmlAdContentToBytes(ClasssifiedDisplayEstimatedCostingViewModel model)
        {
            var imageContentHtml = "";
            if ((model.ImageContent != null && model.ImageContent.Length > 0))
            {
                var bytes = await model.ImageContent.GetBytes();
                var base64String = Convert.ToBase64String(bytes);

                imageContentHtml = $@"
                        <div style='width: 45px; height: 50px; margin: auto;'>
			                <img style='width:43px;height: 46px;margin-top: 3px;float: left;' src='data:image/png;base64,{base64String}' />
		                </div>                    
                ";
            }
            else if (model.ABPrintClassifiedDisplayId > 0)
            {
                //TODO: will be implemented after azure storage implemented
                //var classifiedDisplayDetails = await abPrintClassifiedDisplayService.GetABPrintClassifiedDisplayDetail(model.ABPrintClassifiedDisplayId);

                //if(classifiedDisplayDetails!=null && string.IsNullOrWhiteSpace(classifiedDisplayDetails.OriginalImageUrl))
                //{


                //}

            }

            var converter = new HtmlConverter();
            var htmlAdContent = $@"
                <div style='margin: auto;width: 250px;margin-top: -14px;margin-bottom: -5px;'>	
                    <meta charset='utf-8'>
		            <p style='text-align: center;margin-bottom: -3px;'>
			            <b>{model.Title}</b>
		            </p>		                
			        {imageContentHtml}		                
		            <p style='text-align: justify; margin-top: -1px; margin-bottom: 0px;'>{model.AdContent}</p>	                
                </div>";
            var htmlToImagebytes = converter.FromHtmlString(htmlAdContent, 250, CoreHtmlToImage.ImageFormat.Jpg);
            return htmlToImagebytes;
        }

        private async Task<List<ABPrintClassifiedDisplayDetail>> PopulateABPrintClassifiedDisplayDetail(BookClasssifiedDisplayAdViewModel model, DateTime currentDate)
        {
            var listing = new List<ABPrintClassifiedDisplayDetail>();
            var editionId = CurrentLoginUser.EditionId;

            var editionPage = await editionPageService.GetEdtionPageByEditionAndPageNo(editionId.Value, EditionPagesConstants.Page_7);

            foreach (var date in model.DateBasedOfferList)
            {
                var newClassifiedDisplayDetail = new ABPrintClassifiedDisplayDetail
                {
                    AdContent = model.AdContent,
                    Title = model.Title,
                    EditionId = editionId,
                    EditionPageId = editionPage?.EditionPageId,
                    OriginalImageUrl = model.OriginalImageUrl,
                    FinalImageUrl = model.FinalImageUrl,
                    PublishDate = Convert.ToDateTime(date)
                };

                listing.Add(newClassifiedDisplayDetail);
            }
            return listing;
        }

        private async Task<ABPrintClassifiedDisplay> PopulateClassifiedDisplayInfo(BookClasssifiedDisplayAdViewModel model, string bookingNo, DateTime currentDate)
        {
            decimal discountPercentage = 0, estimatedCosting = 0;
            int offerDateId = 0;
            //fatch date offer
            var offerDate = await FatchDateOffer(model.DateBasedOfferList);
            if (offerDate != null && offerDate.OfferDateId > 0)
            {
                offerDateId = offerDate.OfferDateId;
                discountPercentage = offerDate.DiscountPercentage;
            }

            //fetch estimated total amount
            var responseEstimatedTotal = await FetchEstimatedTotalAmount(model.AdColumnInch);
            if (responseEstimatedTotal.IsSuccess)
                estimatedCosting = responseEstimatedTotal.Result;

            //get total word counts for title
            model.TotalTitleWordCount = CoreHelper.TotalWordCounts(model.Title);

            //get total word counts for ad content
            model.TotalAdContentWordCount = CoreHelper.TotalWordCounts(model.AdContent);

            var newClassifiedDisplay = new ABPrintClassifiedDisplay
            {
                ABPrintClassifiedDisplayId = model.ABPrintClassifiedDisplayId,
                BookingNo = bookingNo,
                CategoryId = model.CategoryId,                
                AdColumnInch = model.AdColumnInch,
                CompleteMatter = model.CompleteMatter,
                SubCategoryId = model.SubCategoryId,
                TotalTitleWordCount = model.TotalTitleWordCount,
                TotalAdContentWordCount = model.TotalAdContentWordCount,
                BookedBy = CurrentLoginUser.UserId,
                BookDate = currentDate,
                GrossTotal = estimatedCosting,
                DiscountPercent = discountPercentage,
                OfferDateId = offerDateId,
                UpazillaId = CurrentLoginUser.UpazillaId,
                AgencyId = model.AgencyId,
                CreatedBy = CurrentLoginUser.UserId,
                AdStatus = AdStatusConstants.Booked,
                BookingStep = BookingStepConstants.Booked,
                IsCorrespondent = CurrentLoginUser.IsCorrespondentUser,
                Remarks = model.Remarks,
            };

            newClassifiedDisplay.DiscountAmount = (newClassifiedDisplay.GrossTotal * newClassifiedDisplay.DiscountPercent) / 100;
            newClassifiedDisplay.NetAmount = newClassifiedDisplay.GrossTotal - newClassifiedDisplay.DiscountAmount;

            decimal vat = 0;
            if (model.EditionId == EditionConstants.National) vat = 15;

            newClassifiedDisplay.VATAmount = (newClassifiedDisplay.NetAmount * vat) / 100;
            newClassifiedDisplay.Commission = 0;

            //agency commission
            if (!model.Personal && model.AgencyId > 0)
            {
                var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                if (agency != null && agency.AgencyId > 0)
                    newClassifiedDisplay.Commission = agency.CDCommission;
            }
            else if (CurrentLoginUser.IsCorrespondentUser)
            {
                newClassifiedDisplay.Commission = CurrentLoginUser.DefaultCommission;
            }

            return newClassifiedDisplay;
        }

        private Advertiser PopulateAdvertiser(BookClasssifiedDisplayAdViewModel model, DateTime currentDate)
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

        private async Task<BaseResponse> ValidateEditForm(BookClasssifiedDisplayAdViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.DatesBasedOffer))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Please add atleast DATE BASED OFFER" };

                if ((model.Personal || CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
                   && (string.IsNullOrWhiteSpace(model.AdvertiserName))
                   )
                    return new BaseResponse { IsSuccess = false, Message = "You must fill advitiser name" };

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

                /*
                if (model.TotalTitleWordCount > model.MaxTitleWords)
                    return new BaseResponse { IsSuccess = false, Message = $"Warning! Title Total Word count exceed the max of {model.MaxTitleWords}." };

                if (model.TotalAdContentWordCount > model.MaxContentWords)
                    return new BaseResponse { IsSuccess = false, Message = $"Warning! Ad Content Total Word count exceed the max of {model.MaxTitleWords}." };
                */

                if (model.CompleteMatter && string.IsNullOrWhiteSpace(model.OriginalImageUrl) && (model.ImageContent == null || model.ImageContent.Length <= 0))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Ad Photo not found. Please try again!" };

                if (!model.CompleteMatter && (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.AdContent)))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Title and Ad Matter is Required." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = "There are error on validating form" };
            }

            return new BaseResponse { IsSuccess = true, Message = "Success" };
        }

        private async Task<BaseResponse> ValidateForm(BookClasssifiedDisplayAdViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.DatesBasedOffer))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Please add atleast DATE BASED OFFER" };

                if ((model.Personal || CurrentLoginUser.UserGroup == UserGroupConstants.Correspondent)
                   && (string.IsNullOrWhiteSpace(model.AdvertiserName))
                   )
                    return new BaseResponse { IsSuccess = false, Message = "You must fill advitiser name" };

                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) &&
                    (
                        (string.IsNullOrWhiteSpace(model.AgencyAutoComplete) || model.AgencyId <= 0) &&
                        ( string.IsNullOrWhiteSpace(model.AdvertiserName))
                    ))
                    return new BaseResponse { IsSuccess = false, Message = "You must fill Agency or Advertiser" };

                if ((CurrentLoginUser.UserGroup == UserGroupConstants.CRM_User) && (!string.IsNullOrWhiteSpace(model.AgencyAutoComplete) && model.AgencyId > 0))
                {
                    var agency = await agencyService.GetDetailsByIdAndName(model.AgencyId, model.AgencyAutoComplete);
                    if (agency == null || agency.AgencyId <= 0)
                        return new BaseResponse { IsSuccess = false, Message = "Agency info modified. Please select agent again!" };
                }

                /*
                if (model.TotalTitleWordCount > model.MaxTitleWords)
                    return new BaseResponse { IsSuccess = false, Message = $"Warning! Title Total Word count exceed the max of {model.MaxTitleWords}." };

                if (model.TotalAdContentWordCount > model.MaxContentWords)
                    return new BaseResponse { IsSuccess = false, Message = $"Warning! Ad Content Total Word count exceed the max of {model.MaxTitleWords}." };
                */

                if (model.CompleteMatter && (model.ImageContent == null || model.ImageContent.Length <= 0))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Ad Photo not found. Please try again!" };

                if (!model.CompleteMatter && (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.AdContent)))
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Title and Ad Matter is Required." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = "There are error on validating form" };
            }

            return new BaseResponse { IsSuccess = true, Message = "Success" };
        }


        #endregion
    }
}
