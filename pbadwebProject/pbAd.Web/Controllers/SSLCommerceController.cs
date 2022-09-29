#region Usings
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Service.ABDigitalDisplays;
using pbAd.Service.ABPrintClassifiedDisplays;
using pbAd.Service.ABPrintClassifiedTexts;
using pbAd.Service.ABPrintPrivateDisplays;
using pbAd.Service.Agencies;
using pbAd.Service.CacheManagerServices;
using pbAd.Service.DefaultDiscounts;
using pbAd.Service.EditionDistricts;
using pbAd.Service.EditionPages;
using pbAd.Service.Editions;
using pbAd.Service.OfferEditions;
using pbAd.Service.RatePrintClassifiedTexts;
using pbAd.Service.SSLPaymentTransactions;
using pbAd.Web.Infrastructure.Helpers;
using pbAd.Web.ViewModels.Checkout;
using pbAd.Web.ViewModels.SSLCommerce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Web.Controllers
{
    public class SSLCommerceController : AdminBaseController
    {
        #region Private Methods

        private readonly ISSLPaymentTransactionService sslPaymentTransactionService;
        private readonly IABPrintClassifiedTextService aBPrintClassifiedTextService;
        private readonly IABPrintClassifiedDisplayService aBPrintClassifiedDisplayService;
        private readonly IABPrintPrivateDisplayService aBPrintPrivateDisplayService;
        private readonly IABDigitalDisplayService aBDigitalDisplayService;
        private readonly ICacheManagerService cacheManagerService;

        #endregion

        #region Ctor

        public SSLCommerceController(ISSLPaymentTransactionService sslPaymentTransactionService,
             IABPrintClassifiedTextService aBPrintClassifiedTextService,
             IABPrintClassifiedDisplayService aBPrintClassifiedDisplayService,
             IABPrintPrivateDisplayService aBPrintPrivateDisplayService,
             ICacheManagerService cacheManagerService,
             IABDigitalDisplayService aBDigitalDisplayService
            )
        {
            this.sslPaymentTransactionService = sslPaymentTransactionService;
            this.aBPrintClassifiedTextService = aBPrintClassifiedTextService;
            this.aBPrintClassifiedDisplayService = aBPrintClassifiedDisplayService;
            this.aBPrintPrivateDisplayService = aBPrintPrivateDisplayService;
            this.aBDigitalDisplayService = aBDigitalDisplayService;
            this.cacheManagerService = cacheManagerService;
        }

        #endregion

        #region Checkout Failed
        
        [AllowAnonymous]
        [Route("/sslcommerce/failed/adtype/{adtype}/{adMasterId}/bookingno/{bookingno}")]
        public async Task<IActionResult> CheckoutFailed(string adtype,string bookingno, int adMasterId)
        {
            var userInfo = cacheManagerService.GetSSLInUserInfo(bookingno);
            ViewBag.SSLUserInfo = userInfo;

            var error = Request.Form["error"];           
            var updateSSLPaymentTransaction = new SSLPaymentTransaction
            {
                AdMasterId = adMasterId,
                AdType=adtype,
                PaymentTransactionStatus = PaymentTransactionStatusConstants.Failed,
                FailedReason = error
            };

            await sslPaymentTransactionService.Update(updateSSLPaymentTransaction);

            if (adtype == AdTypeConstants.ClassifiedText)
            {
                var updateClassifiedText = new ABPrintClassifiedText
                {
                    ABPrintClassifiedTextId = adMasterId,
                    ApprovalStatus = ApproveStatusConstants.Approved,
                    PaymentStatus = PaymentStatusConstants.Not_Paid,
                    AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                };
                //let's update ct
                await aBPrintClassifiedTextService.UpdatePaymentMode(updateClassifiedText);
            }

            if (adtype == AdTypeConstants.ClassifiedDisplay)
            {
                var updateClassifiedDisplay = new ABPrintClassifiedDisplay
                {
                    ABPrintClassifiedDisplayId = adMasterId,
                    ApprovalStatus = ApproveStatusConstants.Approved,
                    PaymentStatus = PaymentStatusConstants.Not_Paid,
                    AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                };
                //let's update cd
                await aBPrintClassifiedDisplayService.UpdatePaymentMode(updateClassifiedDisplay);
            }

            if (adtype == AdTypeConstants.PrivateDisplay)
            {
                var updatePrivateDisplay = new ABPrintPrivateDisplay
                {
                    ABPrintPrivateDisplayId = adMasterId,
                    ApprovalStatus = ApproveStatusConstants.Approved,
                    PaymentStatus = PaymentStatusConstants.Not_Paid,
                    AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                };
                //let's update pd
                await aBPrintPrivateDisplayService.UpdatePaymentMode(updatePrivateDisplay);
            }
            if (adtype == AdTypeConstants.DigitalDisplay)
            {
                var updateABDigitalDisplay = new ABDigitalDisplay
                {
                    ABDigitalDisplayId = adMasterId,
                    ApprovalStatus = ApproveStatusConstants.Approved,
                    PaymentStatus = PaymentStatusConstants.Not_Paid,
                    AdStatus = AdStatusConstants.Confirmed_But_Not_Paid,
                };
                //let's update dd
                await aBDigitalDisplayService.UpdatePaymentMode(updateABDigitalDisplay);
            }

            return View();
        }

        #endregion

        #region Checkout Cancel
                
        [AllowAnonymous]
        [Route("/sslcommerce/cancel/adtype/{adtype}/{adMasterId}/bookingno/{bookingno}")]
        public async Task<IActionResult> CheckoutCancel(string adtype,string bookingno, int adMasterId)
        {
            var userInfo = cacheManagerService.GetSSLInUserInfo(bookingno);
            ViewBag.SSLUserInfo = userInfo;

            var updateSSLPaymentTransaction = new SSLPaymentTransaction
            {
                AdMasterId = adMasterId,
                AdType = adtype,
                PaymentTransactionStatus = PaymentTransactionStatusConstants.Cancelled,
                FailedReason = $"Checkout Cancelled for {adtype} and Master Id {adMasterId}"
            };

            await sslPaymentTransactionService.Update(updateSSLPaymentTransaction);

            return View();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
