using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("BrandRelation")]
    public class BrandRelation
    {
        [Key]
        public int BrandRelationId { get; set; }

        [Display(Name = "Brand")]
        [StringLength(255, ErrorMessage = "Maximum length is {1}")]
        public string Brand { get; set; }

        [Display(Name = "Advertiser Name")]
        [StringLength(255, ErrorMessage = "Maximum length is {1}")]
        public string AdvertiserName { get; set; }

        [Display(Name = "Category Name")]
        [StringLength(255, ErrorMessage = "Maximum length is {1}")]
        public string CategoryName { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Advertiser")]
        public int? AdvertiserId { get; set; }

        [Display(Name = "Brand")]
        public int? BrandId { get; set; }

    }
}