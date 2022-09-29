#region Usings

using AutoMapper;
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABPrintPrivateDisplays;
using pbAd.Data.Models;
using pbAd.Service.ABDigitalDisplays;
using pbAd.Service.ABPrintPrivateDisplays;
using pbAd.Service.AdBookingReports;
using pbAd.Service.Advertisers;

using pbAd.Service.CacheManagerServices;
using pbAd.Service.EmailSenders;
using pbAd.Service.Users;
using pbAd.Web.Infrastructure.Framework;
using pbAd.Web.ViewModels.AdBookingReports;
using pbAd.Web.ViewModels.BookPrivateDisplayAds;
using pbAd.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace pbAd.Web.Controllers
{
    public class UploadLaterController : AdminBaseController
    {
        #region Private Members

        private readonly IAdvertiserService consumerService;
        private readonly IMapper mapper;
        private readonly IWorkContext workContext;
        private readonly ICacheManagerService cacheManagerService;
        private readonly IUserService userService;
        private readonly IEmailSenderService emailSenderService;
        private readonly IConfiguration configuration;
        private readonly IABPrintPrivateDisplayService aBPrintPrivateDisplayService;
        private readonly IABDigitalDisplayService abDigitalDisplayService;
        private readonly IAdBookingReportService adBookingReportService;

        #endregion

        #region Ctor
        public UploadLaterController(
           IAdvertiserService consumerService,
           IWorkContext workContext,
           ICacheManagerService cacheManagerService,
           IUserService userService,
           IEmailSenderService emailSenderService,
           IABPrintPrivateDisplayService aBPrintPrivateDisplayService,
           IAdBookingReportService adBookingReportService,
           IABDigitalDisplayService abDigitalDisplayService,
           IConfiguration configuration,
           IMapper mapper
           )
        {
            this.consumerService = consumerService;
            this.mapper = mapper;
            this.workContext = workContext;
            this.cacheManagerService = cacheManagerService;
            this.userService = userService;
            this.emailSenderService = emailSenderService;
            this.configuration = configuration;
            this.aBPrintPrivateDisplayService = aBPrintPrivateDisplayService;
            this.adBookingReportService = adBookingReportService;
          
            this.abDigitalDisplayService = abDigitalDisplayService;
        }
        #endregion

        #region List

        [HttpGet]
        public async Task<ActionResult> List(int? pageNumber, string searchTerm,
           int totalCount = 0, string sort = "Title", string sortdir = "ASC")
        {
            int bookedBy = CurrentLoginUser.IsCorrespondentUser ? CurrentLoginUser.UserId : 0;
            pageNumber = pageNumber ?? 1;
            if (string.IsNullOrWhiteSpace(searchTerm))
                searchTerm = string.Empty;

            var filter = new AdBookingReportSearchFilter
            {
                BookedBy = bookedBy,
                SearchTerm = searchTerm,
                PageNumber = (int)pageNumber,
                PageSize = 20
            };

            var listing = await adBookingReportService.GetUploadLatersByFilter(filter);

            var model = new AdBookingOrderListViewModel
            {
                UploadLaterOrderList = listing,
                SearchFilter = filter
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("List")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(AdBookingReportSearchFilter filter)
        {
            int bookedBy = CurrentLoginUser.IsCorrespondentUser ? CurrentLoginUser.UserId : 0;

            if (string.IsNullOrWhiteSpace(filter.SearchTerm))
                filter.SearchTerm = string.Empty; 

            filter.BookedBy = bookedBy;
            filter.PageNumber = 1;
            filter.PageSize = 20;
            
            var listing = await adBookingReportService.GetUploadLatersByFilter(filter);

            var model = new AdBookingOrderListViewModel
            {
                UploadLaterOrderList = listing,
                SearchFilter = filter
            };

            return View("List", model);
        }

        #endregion

        #region Upload Later

        [HttpGet]       
        [Route("/uploadlater/adtype/{adType}/{autoId}/bookingno/{bookingNo}")]
        public async Task<IActionResult> Upload(string adType, int autoId, string bookingNo)
        {
            if (autoId <= 0 || string.IsNullOrWhiteSpace(bookingNo)) return Redirect("/myprofile");

            var model = new AdBookingUploadLaterViewModel();

            if (adType != AdTypeConstants.DigitalDisplay)
            {
                var privateDisplay = await aBPrintPrivateDisplayService.GetDetailsByIdAndBookingNo(autoId, bookingNo);

                if(privateDisplay==null) return Redirect("/");

                model = new AdBookingUploadLaterViewModel
                {
                    AutoId = autoId,
                    AdType = adType,
                    BookingNumber = bookingNo,
                    BookedBy = privateDisplay.BookedByUser.FullName,
                    BookingDate = privateDisplay.BookDate,
                    BillDate = privateDisplay.BillDate,
                    NetPayable = (int)(privateDisplay.NetAmount - privateDisplay.Commission + privateDisplay.VATAmount),                  
                };
            }
            else if (adType == AdTypeConstants.DigitalDisplay)
            {
                var digitalDisplay = await abDigitalDisplayService.GetDetailsByIdAndBookingNo(autoId, bookingNo);

                if (digitalDisplay == null) return Redirect("/");

                model = new AdBookingUploadLaterViewModel
                {
                    AutoId = autoId,
                    AdType = adType,
                    BookingNumber = bookingNo,
                    BookedBy = digitalDisplay.BookedByUser.FullName,
                    BookingDate = digitalDisplay.BookDate,
                    BillDate = digitalDisplay.BillDate,
                    NetPayable = (int)(digitalDisplay.NetAmount - digitalDisplay.Commission + digitalDisplay.VATAmount),                   
                };
            }

            return View(model);
        }

        [HttpPost]       
        [Route("/uploadlater/adtype/{adType}/{autoId}/bookingno/{bookingNo}")]
        public async Task<IActionResult> Upload(AdBookingUploadLaterViewModel model)
        {           
            if (!ModelState.IsValid)
                return Json(new { status = false, message = "You must all the required fields. Try again", type = ActionResultTypeConstants.Message });

            try
            {
                //validate form
                var responseValidateForm = ValidateForm(model);
                if (!responseValidateForm.IsSuccess)
                    return Json(new { status = false, message = responseValidateForm.Message, type = ActionResultTypeConstants.Message });

                if (model.AdType == AdTypeConstants.DigitalDisplay)                
                    return await UploadLaterForDigitalAd(model);                
                else                                    
                    return await UploadLaterForPrivateAndGovtAd(model);
                
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error, An Error Occurred while Processing Request. Try again", type = ActionResultTypeConstants.Message });
            }
        }


        #endregion

        #region Ajax Calls       

        #endregion

        #region Private Methods  

        private async Task<IActionResult> UploadLaterForPrivateAndGovtAd(AdBookingUploadLaterViewModel model)
        {
           
            //populate private display
            var privateDisplay = PopulatePrivateDisplayInfo(model);

            var request = new UploadLaterAdProcessModel
            {
                ABPrintPrivateDisplay = privateDisplay
            };

            //let's book private display ad
            var response = await aBPrintPrivateDisplayService.UpdateUploadLater(request);

            if (!response.IsSuccess)
                return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

            var returnUrl = $"/uploadlater/list";

            return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
        }

        private async Task<IActionResult> UploadLaterForDigitalAd(AdBookingUploadLaterViewModel model)
        {
           

            //populate digital display
            var digitalDisplay = PopulateDigitalDisplayInfo(model);

            var request = new UploadLaterAdProcessModel
            {
                ABDigitalDisplay = digitalDisplay
            };

            //let's book digital display ad
            var response = await abDigitalDisplayService.UpdateUploadLater(request);

            if (!response.IsSuccess)
                return Json(new { status = false, message = response.Message, type = ActionResultTypeConstants.Message });

            var returnUrl = $"/uploadlater/list";

            return Json(new { status = true, message = returnUrl, type = ActionResultTypeConstants.Url });
        }

        private BaseResponse ValidateForm(AdBookingUploadLaterViewModel model)
        {
            try
            {               
                if (model.ImageContents == null || !model.ImageContents.Any())
                    return new BaseResponse { IsSuccess = false, Message = "Warning! Uploaded file not found." };
                               
                return new BaseResponse { IsSuccess = true, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        private ABPrintPrivateDisplay PopulatePrivateDisplayInfo(AdBookingUploadLaterViewModel model)
        {
            var newPrivateDisplay = new ABPrintPrivateDisplay
            {
                ABPrintPrivateDisplayId=model.AutoId,
                BookingNo = model.BookingNumber,
                BillDate = model.BillDate,
                UploadLater = false,
                UploadLaterBy = CurrentLoginUser.UserId,
                UploadLaterDate = DateTime.Now,
                ModifiedBy = CurrentLoginUser.UserId
            };
                        
            return newPrivateDisplay;
        }

        private ABDigitalDisplay PopulateDigitalDisplayInfo(AdBookingUploadLaterViewModel model)
        {
            var newDigitalDisplay = new ABDigitalDisplay
            {
                ABDigitalDisplayId = model.AutoId,
                BookingNo = model.BookingNumber,
                BillDate = model.BillDate,
                UploadLater = false,
                UploadLaterBy = CurrentLoginUser.UserId,
                UploadLaterDate = DateTime.Now,
                ModifiedBy = CurrentLoginUser.UserId
            };

            return newDigitalDisplay;
        }

        //private async Task<List<PrivateDisplayMediaContent>> SaveAndGetAdImageContentUrl(AdBookingUploadLaterViewModel model)
        //{
        //    var privateDisplayMediaContents = new List<PrivateDisplayMediaContent>();

        //    string[] removeUploadedFiles = new string[] { };

        //    if (!string.IsNullOrWhiteSpace(model.RemoveUploadedFiles))
        //        removeUploadedFiles = model.RemoveUploadedFiles.Split(new[] { "@@AdPro@@" }, StringSplitOptions.None);
        //    int serialNo = 1;

        //    foreach (var adContent in model.ImageContents)
        //    {
        //        if (removeUploadedFiles.Any(f => f == adContent.FileName))
        //            continue;

        //        var fileName = $"{model.BookingNumber}-pd-ad-content{Path.GetExtension(adContent.FileName)}";
        //        var blobContainer = configuration["AzureService:Container:PrivateDisplay"];
        //        if (model.AdType == AdTypeConstants.GovtDisplay)
        //        {
        //            fileName = $"{model.BookingNumber}-gd-ad-content{Path.GetExtension(adContent.FileName)}";
        //            blobContainer = configuration["AzureService:Container:GovtAd"];
        //        }

        //        var request = new AzureFileUploadRequestModel
        //        {
        //            FileContent = adContent,
        //            Filename = fileName,
        //            BlobContainer = blobContainer,
        //            BlobServiceClient = configuration["AzureService:BlobServiceClient"],
        //            BlobContentType = adContent.ContentType,
        //            IsIFormFile = true
        //        };

        //        // let's save to azure storage
        //        var responseAzureStorage = await azureServiceService.Upload(request);

        //        var dbSavePath = responseAzureStorage.AbsoluteUri;

        //        privateDisplayMediaContents.Add(new PrivateDisplayMediaContent
        //        {
        //            ABPrintPrivateDisplayId = model.AutoId,
        //            OriginalImageUrl = dbSavePath,
        //            CreatedDate = DateTime.Now
        //        });

        //        serialNo = serialNo + 1;
        //    }

        //    return privateDisplayMediaContents;
        //}



        #endregion
    }
}
