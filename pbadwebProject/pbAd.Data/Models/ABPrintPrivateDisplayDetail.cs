
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("ABPrintPrivateDisplayDetail")]
    public class ABPrintPrivateDisplayDetail
    {
        [Key]
        public int ABPrintPrivateDisplayDetailId { get; set; }

        [Display(Name = "AB Print Private Display")]
        public int? ABPrintPrivateDisplayId { get; set; }

        [Display(Name = "Edition")]
        public int EditionId { get; set; }

        [Display(Name = "Edition Page")]
        public int EditionPageId { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Content Url")]
        [StringLength(450, ErrorMessage = "Maximum length is {1}")]
        public string ContentUrl { get; set; }
    }
}