using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("Agency")]
    public class Agency
    {
        [Key]

        public int AgencyId { get; set; }

        [Display(Name = "Agency Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string AgencyName { get; set; }

        [Display(Name = "Credit Limit")]
        public int CreditLimit { get; set; }

        [Display(Name = "PD Commission")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal PDCommission { get; set; }

        [Display(Name = "CT Commission")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal CTCommission { get; set; }

        [Display(Name = "CD Commission")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal CDCommission { get; set; }

        [Display(Name = "GD Commission")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal GDCommission { get; set; }

        [Display(Name = "DD Commission")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal DDCommission { get; set; }


        [Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string EmailAddress { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
        public int? CollectorId { get; set; }
    }
}