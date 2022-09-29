
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("RatePrintClassifiedText")]
    public class RatePrintClassifiedText
    {
        [Key]
        public int AutoId { get; set; }

        [Display(Name = "Edition")]
        public int EditionId { get; set; }

        [Display(Name = "Per Word Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PerWordRate { get; set; }

        [Display(Name = "Big Bullet Point Single")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal BigBulletPointSingle { get; set; }

        [Display(Name = "Big Bullet Point Double")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal BigBulletPointDouble { get; set; }

        [Display(Name = "Bold Percentage")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal BoldPercentage { get; set; }

        [Display(Name = "Boldin Screen Percentage")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal BoldinScreenPercentage { get; set; }

        [Display(Name = "Bold Screen Single Box Percentage")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal BoldScreenSingleBoxPercentage { get; set; }

        [Display(Name = "Bold Screen Double Box Percentage")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal BoldScreenDoubleBoxPercentage { get; set; }

        [Display(Name = "Max Words")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MaxWords { get; set; }

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