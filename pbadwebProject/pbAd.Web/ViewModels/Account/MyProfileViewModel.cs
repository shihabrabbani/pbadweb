
#region Using
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace pbAd.Web.ViewModels.Account
{
    public class MyProfileViewModel
    {        
        [Display(Name = "Advertiser Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string AdvertiserName { get; set; }        

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is Required")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email")]
        [StringLength(300, ErrorMessage = "Maximum length is {1}")]
        public string Email { get; set; }        

        public int Gender { get; set; }
        public DateTime DateofBirth { get; set; }

        [Display(Name = "Cell Phone")]
        [StringLength(25, ErrorMessage = "Maximum length is {1}")]
        public string CellPhone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }    
    }
}