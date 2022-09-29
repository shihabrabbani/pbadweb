
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("tblMenu")]
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }

        [Display(Name = "Module")]
        public int? ModuleId { get; set; }

        [Display(Name = "Page Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string PageName { get; set; }

        [Display(Name = "Display Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string DisplayName { get; set; }

        [Display(Name = "Pageid")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Pageid { get; set; }

        [Display(Name = "U R L")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string URL { get; set; }

        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Parent")]
        public int? Parent { get; set; }

        [Display(Name = "Icon File Path")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string IconFilePath { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}