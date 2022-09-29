using Microsoft.AspNetCore.Authentication;
using pbAd.Core.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        //external logins
        public IList<AuthenticationSchemeModel> ExternalLogins => new AuthenticationSchemeModel().GetAuthenticationSchemes();
    }    
}