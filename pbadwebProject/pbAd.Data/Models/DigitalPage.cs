
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("DigitalPage")]
    public class DigitalPage
    {
        [Key]
        public int DigitalPageId { get; set; }

        [Display(Name = "Digital Page Name")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string DigitalPageName { get; set; }

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