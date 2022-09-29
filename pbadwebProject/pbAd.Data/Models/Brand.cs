using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("Brand")]
    public class Brand
    {
        [Key]

        public int BrandId { get; set; }

        [Display(Name = "Brand Name")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string BrandName { get; set; }

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