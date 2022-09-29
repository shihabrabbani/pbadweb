
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("OfferDate")]
    public class OfferDate
    {
        [Key]
        public int OfferDateId { get; set; }

        [Display(Name = "Date Offer Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string DateOfferName { get; set; }

        [Display(Name = "Days Range From")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DaysRangeFrom { get; set; }

        [Display(Name = "Days Range To")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DaysRangeTo { get; set; }

        [Display(Name = "Noof Time From")]
        [Required(ErrorMessage = "{0} is Required")]
        public int NoofTimeFrom { get; set; }

        [Display(Name = "Noof Time To")]
        [Required(ErrorMessage = "{0} is Required")]
        public int NoofTimeTo { get; set; }

        [Display(Name = "Discount Percentage")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal DiscountPercentage { get; set; }

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
    }
}