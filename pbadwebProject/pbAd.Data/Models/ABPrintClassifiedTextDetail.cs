
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("ABPrintClassifiedTextDetail")]
    public class ABPrintClassifiedTextDetail
    {
        [Key]
        public int ABPrintClassifiedTextDetailId { get; set; }

        [Display(Name = "AB Print Classified Text")]
        public int? ABPrintClassifiedTextId { get; set; }

        [Display(Name = "Ad Content")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string AdContent { get; set; }

        [Display(Name = "Edition")]
        public int? EditionId { get; set; }

        [Display(Name = "Edition Page")]
        public int? EditionPageId { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

    }
}