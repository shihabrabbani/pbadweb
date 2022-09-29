using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace pbAd.Data.Models
{
    [Table("SubCategory")]
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage ="{0} is Required")]
        public int CategoryId { get; set; }

        [Display(Name = "Sub Category Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string SubCategoryName { get; set; }

        //[Display(Name = "Created Date")]
        //[Required(ErrorMessage = "{0} is Required")]
        //public DateTime? CreatedDate { get; set; }

        //[Display(Name = "Created By")]
        //[Required(ErrorMessage = "{0} is Required")]
        //public int? CreatedBy { get; set; }

        //[Display(Name = "Modified Date")]
        //public DateTime? ModifiedDate { get; set; }

        //[Display(Name = "Modified By")]
        //public int? ModifiedBy { get; set; }

    }
}
