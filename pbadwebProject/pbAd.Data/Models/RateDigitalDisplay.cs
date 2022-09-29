
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("RateDigitalDisplay")]
    public class RateDigitalDisplay
    {
        [Key]
        public int AutoId { get; set; }

        [Display(Name = "Digital Ad Unit Type")]
        public int? DigitalAdUnitTypeId { get; set; }

        [Display(Name = "Per Unit Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PerUnitRate { get; set; }

        [Display(Name = "Digital Page")]
        public int? DigitalPageId { get; set; }

        [Display(Name = "Digital Page Position")]
        public int? DigitalPagePositionId { get; set; }

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