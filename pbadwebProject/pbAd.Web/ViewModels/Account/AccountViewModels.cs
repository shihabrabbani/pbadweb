using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Account
{
    public class ChangePasswordModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is Required")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "OTP")]
        public string OTP { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "{0} is Required")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsOneTimePassword { get; set; }
    }

    public class OTPRequestModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is Required")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required")]
        public string Email { get; set; }
    }
}
