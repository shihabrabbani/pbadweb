
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("EditionPage")]
    public class EditionPage
    {
        [Key]
        public int EditionPageId { get; set; }

        [Display(Name = "Edition")]
        public int EditionId { get; set; }

        [Display(Name = "Edition Page Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string EditionPageName { get; set; }

        [Display(Name = "Edition Page No")]
        public int EditionPageNo { get; set; }

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