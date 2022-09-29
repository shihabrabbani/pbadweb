
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("RatePrintGovtAd")]
    public class RatePrintGovtAd
    {
        [Key]
        public int AutoId { get; set; }

        [Display(Name = "Corp Color Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal CorpColorRate { get; set; }

        [Display(Name = "Corp BW Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal CorpBWRate { get; set; }

        [Display(Name = "Per Column Inch Color Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PerColumnInchColorRate { get; set; }

        [Display(Name = "Per Column Inch BW Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PerColumnInchBWRate { get; set; }

        [Display(Name = "Edition Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EditionId { get; set; }

        [Display(Name = "Edition Page Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EditionPageId { get; set; }

        [Display(Name = "Created By")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }
    }
}