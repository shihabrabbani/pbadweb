using Microsoft.AspNetCore.Authentication;
using pbAd.Core.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Phone/Email")]
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Password")]
        public string AccountVerificationType { get; set; }

        [Display(Name = "Verification Code")]
        [Required(ErrorMessage = "{0} is required.")]
        public string VerificationCode { get; set; }        
    }    
}