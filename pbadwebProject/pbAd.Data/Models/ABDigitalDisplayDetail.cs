

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("ABDigitalDisplayDetail")]
    public class ABDigitalDisplayDetail
    {
        [Key]
        public int ABDigitalDisplayDetailId { get; set; }

        [Display(Name = "Digital Display")]
        [Required(ErrorMessage = "{0} is Required")]
        public int ABDigitalDisplayId { get; set; }

        [Display(Name = "Digital Ad Unit Type")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DigitalAdUnitTypeId { get; set; }

        [Display(Name = "Digital Page")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DigitalPageId { get; set; }

        [Display(Name = "Digital Page Position")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DigitalPagePositionId { get; set; }

        [Display(Name = "Ad Qty")]
        [Required(ErrorMessage = "{0} is Required")]
        public int AdQty { get; set; }

        [Display(Name = "Publish Date Start")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime PublishDateStart { get; set; }

        [Display(Name = "Publish Date End")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime PublishDateEnd { get; set; }

        public decimal PerUnitRate { get; set; }

        //additional
        [ForeignKey("DigitalAdUnitTypeId")]
        public DigitalAdUnitType DigitalAdUnitType { get; set; }

        [ForeignKey("DigitalPageId")]
        public DigitalPage DigitalPage { get; set; }

        [ForeignKey("DigitalPagePositionId")]
        public DigitalPagePosition DigitalPagePosition { get; set; }

        [NotMapped]
        public string PublishDateStartInText { get; set; }

        [NotMapped]
        public string PublishTimeStartInText { get; set; }

        [NotMapped]
        public string PublishDateEndInText { get; set; }

        [NotMapped]
        public string PublishTimeEndInText { get; set; }
    }
}