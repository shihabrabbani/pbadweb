#region Usings

using pbAd.Web.ViewModels.Account;
using pbAd.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace pbAd.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Private Variables

        #endregion

        #region Ctor
        public HomeController()
        {

        }
        #endregion

        #region Index

        public IActionResult Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
                return Redirect("/dashboard");

            var model = new LoginViewModel
            {
                //Username = "admin",
                //Password = "123456"
            };

            return View(model);
        }

        #endregion

        #region Error

        public IActionResult Error()
        {
            return View();
        }

        #endregion

        #region Terms

        [Route("termsandconditions")]
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        #endregion 
    }
}
