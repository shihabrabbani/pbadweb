
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace pbAd.Data.Models
{
    [Table("UserGroup")]
    public class UserGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Display(Name = "Group Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string GroupName { get; set; }
    }
}