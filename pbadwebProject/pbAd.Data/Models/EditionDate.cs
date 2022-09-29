
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("EditionDate")]
    public class EditionDate
    {
        [Key]
        public int EditionDateId { get; set; }

        [Display(Name = "Noof Edition")]
        public int? NoofEdition { get; set; }

        [Display(Name = "Discount Percentage")]
        public decimal? DiscountPercentage { get; set; }

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