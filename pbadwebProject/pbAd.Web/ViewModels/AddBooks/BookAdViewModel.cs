using pbAd.Data.Models;
using pbAd.Web.ViewModels.Account;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.ViewModels.AddBooks
{
    public class BookAdViewModel
    {
        public int ABPrintClassifiedTextId { get; set; }

        public string BookingNo { get; set; }

        [Display(Name = "Advertiser")]
        public string AdvertiserName { get; set; }

        [Display(Name = "Advertiser Contact Number")]
        public string AdvertiserContactNumber { get; set; }

        [Display(Name = "Ad Content")]
        [Required(ErrorMessage = "{0} is Required")]
        public string AdContent { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CategoryId { get; set; }

        [Display(Name = "Brand")]
        public int BrandId { get; set; }

        [Display(Name = "Agency")]
        public int AgencyId { get; set; }

        [Display(Name = "Sub Category")]
        [Required(ErrorMessage = "{0} is Required")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Ad Enhancement Type")]
        public string AdEnhancementType { get; set; }

        [Display(Name = "Ad Enhancement Type (Bullet)")]       
        public string AdEnhancementTypeBullet { get; set; }

        [Display(Name = "Total Word Count")]
        [Required(ErrorMessage = "{0} is Required")]
        public int TotalWordCount { get; set; }

        [Display(Name = "Estimated Total")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal EstimatedTotal { get; set; }

        [Display(Name = "Discount Percentage")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal DiscountPercentage { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }

        public string DatesBasedOffer { get; set; }
        public string[] DateBasedOfferList { get; set; }
        public int OfferDateId { get; set; }

        //addtional
        public string AgencyAutoComplete { get; set; }
        public IEnumerable<SelectListItem> CategoryDropdownList { get; set; }
        public IEnumerable<SelectListItem> SubCategoryDropdownList { get; set; }
        //public IEnumerable<SelectListItem> BrandDropdownList { get; set; }
        //public IEnumerable<SelectListItem> AgencyDropdownList { get; set; }
        public RatePrintClassifiedText RatePrintClassifiedText { get; set; }
        public int EditionId { get; set; }
        public bool Personal { get; set; }
        public bool RequiredAdvertiser { get; set; }
        public List<string> BasedOfferDates { get; set; }
        public Agency AgencyInfo { get; set; }
        public Advertiser AdvertiserInfo { get; set; }
    }
}
