
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace pbAd.Data.Models
{
    [Table("Upazilla")]
    public class Upazilla
    {
        [Key]
        public int UpazillaId { get; set; }

        [Display(Name = "Upazilla Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string UpazillaName { get; set; }

        [Display(Name = "District")]
        public int? DistrictId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}