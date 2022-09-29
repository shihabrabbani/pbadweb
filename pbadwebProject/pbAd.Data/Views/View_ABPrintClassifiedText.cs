

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Views
{
    [Table("View_ABPrintClassifiedText")]
    public class View_ABPrintClassifiedText
    {
        [Key]
        public int ABPrintClassifiedTextId { get; set; }

       
        public string BookingNo { get; set; }

       
        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

       
       
        public int? AdvertiserId { get; set; }

        public string AdvertiserName { get; set; }
        public string AdvertiserMobileNo { get; set; }

      
        [Display(Name = "Agency")]
        public int? AgencyId { get; set; }

        public string AgencyName { get; set; }

    }
}