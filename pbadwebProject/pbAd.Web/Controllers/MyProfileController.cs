#region Usings

using AutoMapper;
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABPrintPrivateDisplays;
using pbAd.Data.Models;
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
    public class MyProfileController : AdminBaseController
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
        private readonly IAdBookingReportService adBookingReportService;
       
        #endregion

        #region Ctor
        public MyProfileController(
           IAdvertiserService consumerService,
           IWorkContext workContext,
           ICacheManagerService cacheManagerService,
           IUserService userService,
           IEmailSenderService emailSenderService,
           IABPrintPrivateDisplayService aBPrintPrivateDisplayService,
           IAdBookingReportService adBookingReportService,

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
           
        }
        #endregion       

        #region My Profile

        [HttpGet]       
        [Route("/myprofile")]
        public async Task<IActionResult> Profile()
        {
            var userInfo = await cacheManagerService.GetLoggedInUser();

            if (userInfo == null || userInfo.UserId <= 0) return Redirect("/account/login");

            var model = mapper.Map<UserViewModel>(userInfo);

            return View(model);
        }

        #endregion
               

        #region Ajax Calls

        [HttpPost]
        public async Task<ActionResult> GetOrderByFilter(string searchTerm = "")
        {
            int bookedBy = CurrentLoginUser.IsCorrespondentUser ? CurrentLoginUser.UserId : 0;

            if (string.IsNullOrWhiteSpace(searchTerm))
                searchTerm = string.Empty;

            var filter = new AdBookingReportSearchFilter
            {
                BookedBy = bookedBy,
                SearchTerm = searchTerm,
                PageNumber = 1,
                PageSize = 20
            };
            var listing = await adBookingReportService.GetListByFilter(filter);

            var model = new AdBookingOrderListViewModel
            {
                BookingOrderList = listing,
                SearchFilter = filter
            };

            return PartialView("_GetOrdersPartial", model);
        }

        #endregion

        #region Private Methods       

        #endregion
    }
}
