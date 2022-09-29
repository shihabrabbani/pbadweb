
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("OfferEdition")]
    public class OfferEdition
    {
        [Key]
        public int EditionDateId { get; set; }

        [Display(Name = "No of Edition")]
        [Required(ErrorMessage = "{0} is Required")]
        public int NoofEdition { get; set; }

        [Display(Name = "Discount Percentage")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal DiscountPercentage { get; set; }

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

        [Display(Name = "Created By")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}