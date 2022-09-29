
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("DigitalPagePosition")]
    public class DigitalPagePosition
    {
        [Key]
        public int DigitalPagePositionId { get; set; }

        public int DigitalPageId { get; set; }

        [Display(Name = "Digital Page Position Name")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string DigitalPagePositionName { get; set; }

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