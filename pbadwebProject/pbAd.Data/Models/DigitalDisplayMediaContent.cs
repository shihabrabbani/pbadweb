

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("DigitalDisplayMediaContent")]
    public class DigitalDisplayMediaContent
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Digital Display")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DigitalDisplayId { get; set; }

        [Display(Name = "Screen Type")]
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        public string ScreenType { get; set; }

        [Display(Name = "Original Image Url")]
        [StringLength(450, ErrorMessage = "Maximum length is {1}")]
        public string OriginalImageUrl { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("DigitalDisplayId")]
        public virtual ABDigitalDisplay ABDigitalDisplay { get; set; }
    }
}