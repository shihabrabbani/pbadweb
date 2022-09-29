
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("tblModule")]
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }

        [Display(Name = "Module Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string ModuleName { get; set; }

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

        [Display(Name = "Module Icon")]
        [StringLength(30, ErrorMessage = "Maximum length is {1}")]
        public string ModuleIcon { get; set; }
    }
}