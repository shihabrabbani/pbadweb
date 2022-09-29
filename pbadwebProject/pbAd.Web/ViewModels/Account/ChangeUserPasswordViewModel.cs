using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Account
{
    public class ChangeUserPasswordViewModel
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsOneTimePassword { get; set; }

        public string PasswordResetToken { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Display(Name = "Email Or Phone")]
        [Required(ErrorMessage = "{0} is Required")]
        public string EmailOrPhone { get; set; }

        [Display(Name = "Verification Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string VerificationCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
    public class VarifyCodeViewModel
    {
        [Display(Name = "Enter The Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string Code { get; set; }
    }
}