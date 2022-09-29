
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("District")]
    public class District
    {
        [Key]

        public int DistrictId { get; set; }

        [Display(Name = "District Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string DistrictName { get; set; }

        [Display(Name = "District Name Eng")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string DistrictNameEng { get; set; }
    }
}