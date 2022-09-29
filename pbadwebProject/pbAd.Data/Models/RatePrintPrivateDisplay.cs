
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("RatePrintPrivateDisplay")]
    public class RatePrintPrivateDisplay
    {
        [Key]
        public int AutoId { get; set; }

        [Display(Name = "Per Column Inch Color Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PerColumnInchColorRate { get; set; }

        [Display(Name = "Per Column Inch B/W Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PerColumnInchBWRate { get; set; }

        [Display(Name = "Edition")]
        public int EditionId { get; set; }

        [Display(Name = "Edition Page")]
        public int? EditionPageId { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }
    }
}