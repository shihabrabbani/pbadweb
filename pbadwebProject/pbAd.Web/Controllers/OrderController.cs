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
    public class OrderController : AdminBaseController
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
        public OrderController(
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

        #region List

        [HttpGet]
        public async Task<ActionResult> List(int? pageNumber, string searchTerm, DateTime? startDate, DateTime? endDate,
            DateTime? billStartDate, DateTime? billEndDate,
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
                StartDate = startDate,
                EndDate = endDate,
                BillStartDate = billStartDate,
                BillEndDate = billEndDate,
                PageNumber = (int)pageNumber,
                PageSize = 20
            };

            var listing = await adBookingReportService.GetListByFilter(filter);

            var model = new AdBookingOrderListViewModel
            {
                BookingOrderList = listing,
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

            filter.SearchTerm = filter.SearchTerm ?? string.Empty;

            filter.BookedBy = bookedBy;
            filter.PageNumber = 1;
            filter.PageSize = 20;

            var listing = await adBookingReportService.GetListByFilter(filter);

            var model = new AdBookingOrderListViewModel
            {
                BookingOrderList = listing,
                SearchFilter = filter
            };

            return View("List", model);
        }

        #endregion

        #region Private Methods       

        #endregion
    }
}
