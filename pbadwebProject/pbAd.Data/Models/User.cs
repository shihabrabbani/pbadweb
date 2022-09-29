using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace pbAd.Data.Models
{
    [Table("tblUser")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }

        [Display(Name = "User Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string UserName { get; set; }

        [Display(Name = "BUser Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string BUserName { get; set; }

        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string FullName { get; set; }

        [Display(Name = "Mobile No")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        public string MobileNo { get; set; }

        [Display(Name = "Password Hash")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string PasswordHash { get; set; }

        [Display(Name = "Password Salt")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string PasswordSalt { get; set; }

        [Display(Name = "Password Reset Token")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string PasswordResetToken { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Email { get; set; }

        [Display(Name = "Designation")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Designation { get; set; }

        [Display(Name = "Edition")]
        public int? EditionId { get; set; }

        [Display(Name = "District")]
        public int DistrictId { get; set; }

        [Display(Name = "Upazilla")]
        public int UpazillaId { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Group Id")]
        public int? GroupId { get; set; }

        [Display(Name = "Default Commission")]
        public decimal DefaultCommission { get; set; }

        public string OTP { get; set; }
        public DateTime? OTPSentTime { get; set; }
        public DateTime? OTPExpireTime { get; set; }


        [NotMapped]
        public string UserPassword { get; set; }
        [NotMapped]
        public bool IsExternalLogin { get; set; }

        [NotMapped]
        public int UserGroup { get; set; }

        [NotMapped]
        public bool IsCRMUser { get; set; }

        [NotMapped]
        public bool IsCorrespondentUser { get; set; }
        
    }
}
