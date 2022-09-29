using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.ViewModels.Users
{
    public class UserViewModel
    {
        public int UserId { get; set; }       
        public int RoleId { get; set; }        
        public string UserName { get; set; }       
        public string FullName { get; set; }        
        public string MobileNo { get; set; }        
        public bool IsActive { get; set; }      
        public int CreatedBy { get; set; }      
        public DateTime CreatedDate { get; set; }       
        public string Email { get; set; }
        public string Designation { get; set; }        
        public int? EditionId { get; set; }        
        public int DistrictId { get; set; }        
        public int UpazillaId { get; set; }        
        public int? GroupId { get; set; }       
        public decimal DefaultCommission { get; set; }         
        public int UserGroup { get; set; }       
        public bool IsCRMUser { get; set; }       
        public bool IsCorrespondentUser { get; set; }
        public string RoleName { get; set; }
    }
}
