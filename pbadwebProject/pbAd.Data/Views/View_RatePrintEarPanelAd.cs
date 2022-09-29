using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace pbAd.Data.Views
{
    [Table("View_RatePrintEarPanelAd")]
    public class View_RatePrintEarPanelAd
    {
        [Key]

        [Display(Name = "Auto Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int AutoId { get; set; }

        [Display(Name = "Rate")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal Rate { get; set; }

        [Display(Name = "Edition Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EditionId { get; set; }

        [Display(Name = "Edition Page Id")]
        public int? EditionPageId { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }
    
        public int EditionPageNo { get; set; }
    }
}
