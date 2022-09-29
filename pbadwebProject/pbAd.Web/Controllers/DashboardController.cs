using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.Models;
using pbAd.Service.ABPrintClassifiedTexts;
using pbAd.Service.Categories;
using pbAd.Service.SubCategories;
using pbAd.Web.ViewModels.AddBooks;
using pbAd.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.Controllers
{
    public class DashboardController : AdminBaseController
    {
        #region Private Methods

        #endregion

        #region Ctor

        public DashboardController()
        {
            
        }

        #endregion

        #region Index

        [Route("/dashboard")]
        public IActionResult Index()
        {
            var model = new DashboardHomeViewModel();

            return View(model);
        }

        #endregion

        #region Ajax Calls


        #endregion

        #region Private Methods


        #endregion        
    }
}
