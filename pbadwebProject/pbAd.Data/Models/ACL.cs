
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("tblACL")]
    public class ACL
    {
        [Key]
        public int ACLId { get; set; }

        [Display(Name = "Role")]
        public int? RoleId { get; set; }

        [Display(Name = "Menu")]
        public int? MenuId { get; set; }

        [Display(Name = "Form Create")]
        public bool? FormCreate { get; set; }

        [Display(Name = "Form View")]
        public bool? FormView { get; set; }

        [Display(Name = "Form Edit")]
        public bool? FormEdit { get; set; }

        [Display(Name = "Form Delete")]
        public bool? FormDelete { get; set; }

        [Display(Name = "Report")]
        public bool? Report { get; set; }

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "is Show")]
        public bool? isShow { get; set; }
    }
}