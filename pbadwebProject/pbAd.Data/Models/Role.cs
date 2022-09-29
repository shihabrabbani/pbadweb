
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("tblRole")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Display(Name = "Role Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string RoleName { get; set; }

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Mother Company Id")]
        public int? MotherCompanyId { get; set; }
    }
}