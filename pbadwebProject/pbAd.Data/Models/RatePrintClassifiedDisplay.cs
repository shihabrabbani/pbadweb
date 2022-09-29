
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("RatePrintClassifiedDisplay")]
    public class RatePrintClassifiedDisplay
    {
        [Key]
        public int AutoId { get; set; }

        [Display(Name = "Edition")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EditionId { get; set; }

        [Display(Name = "Per Column Inch")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PerColumnInch { get; set; }

        [Display(Name = "Max Title Words")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MaxTitleWords { get; set; }

        [Display(Name = "Max Content Words")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MaxContentWords { get; set; }

        [Display(Name = "Rate1 Col Inch")]
        public decimal Rate1ColInch { get; set; }

        [Display(Name = "Max Word1 Col Inch")]
        public int? MaxWord1ColInch { get; set; }

        [Display(Name = "Rate15 Col Inch")]
        public decimal Rate15ColInch { get; set; }

        [Display(Name = "Max Word15 Col Inch")]
        public int? MaxWord15ColInch { get; set; }

        [Display(Name = "Rate2 Col Inch")]
        public decimal Rate2ColInch { get; set; }

        [Display(Name = "Max Word2 Col Inch")]
        public int? MaxWord2ColInch { get; set; }

        [Display(Name = "Rate25 Col Inch")]
        public decimal Rate25ColInch { get; set; }

        [Display(Name = "Max Word25 Col Inch")]
        public int? MaxWord25ColInch { get; set; }

        [Display(Name = "Rate3 Col Inch")]
        public decimal Rate3ColInch { get; set; }

        [Display(Name = "Max Word3 Col Inch")]
        public int? MaxWord3ColInch { get; set; }

        [Display(Name = "Rate35 Col Inch")]
        public decimal Rate35ColInch { get; set; }

        [Display(Name = "Max Word35 Col Inch")]
        public int? MaxWord35ColInch { get; set; }

        [Display(Name = "Rate4 Col Inch")]
        public decimal Rate4ColInch { get; set; }

        [Display(Name = "Max Word4 Col Inch")]
        public int? MaxWord4ColInch { get; set; }

        [Display(Name = "Created By")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }
    }
}