
#region Using
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace pbAd.Web.ViewModels.Account
{
    public class AccountRegisterViewModel
    {
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "{0} is Required")]
        public string FullName { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "{0} is Required")]
        public string UserName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} is Required")]
        public string CellPhone { get; set; }
        
        [Display(Name = "SMS Verification Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string SMSVerificationCode { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Accept Terms & Condition")]
        //[EnforceTrue(ErrorMessage = "You must Accept Terms & Condition to Continue!")]
        public bool AcceptTerms { get; set; }   
        public bool EnableNewsletter { get; set; }     

        public string ReturnUrl { get; set; }
        public string ErrorMessge { get; set; }

        //additional
        //external logins
        public IList<AuthenticationSchemeModel> ExternalLogins => new AuthenticationSchemeModel().GetAuthenticationSchemes();
    }
}