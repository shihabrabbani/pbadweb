
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("EditionDistrict")]
    public class EditionDistrict
    {
        [Key]
        public int AutoId { get; set; }

        [Display(Name = "Edition")]
        public int? EditionId { get; set; }

        [Display(Name = "Hover Text Value")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string HoverTextValue { get; set; }
    }
}