
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("DigitalAdUnitType")]
    public class DigitalAdUnitType
    {
        [Key]
        public int DigitalAdUnitTypeId { get; set; }

        [Display(Name = "Digital Ad Unit Type Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string DigitalAdUnitTypeName { get; set; }

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