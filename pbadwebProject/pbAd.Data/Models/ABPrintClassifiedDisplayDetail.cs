
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("ABPrintClassifiedDisplayDetail")]
    public class ABPrintClassifiedDisplayDetail
    {
        [Key]
        public int ABPrintClassifiedDisplayDetailId { get; set; }

        [Display(Name = "AB Print Classified Display")]
        public int ABPrintClassifiedDisplayId { get; set; }

        [Display(Name = "Title")]
        //[Required(ErrorMessage = "{0} is Required")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string Title { get; set; }

        [Display(Name = "Ad Content")]
        //[Required(ErrorMessage = "{0} is Required")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string AdContent { get; set; }

        [Display(Name = "Edition")]
        public int? EditionId { get; set; }

        [Display(Name = "Edition Page")]
        public int? EditionPageId { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Original Image Url")]
        [StringLength(450, ErrorMessage = "Maximum length is {1}")]
        public string OriginalImageUrl { get; set; }

        [Display(Name = "Final Image Url")]
        [StringLength(450, ErrorMessage = "Maximum length is {1}")]
        public string FinalImageUrl { get; set; }
    }
}