
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("DefaultDiscount")]
    public class DefaultDiscount
    {
        [Key]
        public int DefaultDiscountId { get; set; }

        [Display(Name = "Ad Type")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string AdType { get; set; }

        [Display(Name = "Payment Mode")]
        public int PaymentModeId { get; set; }

        [Display(Name = "Discount Rate")]
        public decimal DiscountRate { get; set; }
    }
}