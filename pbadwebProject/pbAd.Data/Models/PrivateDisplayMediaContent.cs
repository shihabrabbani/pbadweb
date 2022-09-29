using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace pbAd.Data.Models
{
    [Table("PrivateDisplayMediaContent")]
    public class PrivateDisplayMediaContent
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "AB Print Private Display")]
        public int? ABPrintPrivateDisplayId { get; set; }

        [Display(Name = "Original Image Url")]
        [StringLength(450, ErrorMessage = "Maximum length is {1}")]
        public string OriginalImageUrl { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }


        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        //additional

        [ForeignKey("ABPrintPrivateDisplayId")]
        public virtual ABPrintPrivateDisplay ABPrintPrivateDisplay { get; set; }

        public string FileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OriginalImageUrl))
                    return "";

                var filename = Path.GetFileName(OriginalImageUrl);

                return filename;
            }
        }
    }
}