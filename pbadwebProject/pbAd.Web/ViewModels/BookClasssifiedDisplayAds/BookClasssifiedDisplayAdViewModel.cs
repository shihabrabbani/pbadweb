using pbAd.Data.Models;
using pbAd.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.ViewModels.BookClasssifiedDisplayAds
{
    public class BookClasssifiedDisplayAdViewModel
    {
        public int ABPrintClassifiedDisplayId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Advertiser Name")]
        public string AdvertiserName { get; set; }

        [Display(Name = "Advertiser Contact Number")]
        public string AdvertiserContactNumber { get; set; }

        [Display(Name = "Ad Content")]
        public string AdContent { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CategoryId { get; set; }

        [Display(Name = "Sub Category")]
        [Required(ErrorMessage = "{0} is Required")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Total Title Word Count")]
        [Required(ErrorMessage = "{0} is Required")]
        public int TotalTitleWordCount { get; set; }

        [Display(Name = "Total Ad Content Word Count")]
        [Required(ErrorMessage = "{0} is Required")]
        public int TotalAdContentWordCount { get; set; }

        [Display(Name = "Estimated Total")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal EstimatedTotal { get; set; }

        public string DatesBasedOffer { get; set; }
        public string[] DateBasedOfferList { get; set; }

        [Display(Name = "Agency")]
        public int AgencyId { get; set; }

        [Display(Name = "AdColumnInch")]
        public decimal AdColumnInch { get; set; }

        //addtional
        public string AgencyAutoComplete { get; set; }

        [Display(Name = "Upload Image")]
        public IFormFile ImageContent { get; set; }

        public decimal PerColumnInchRate { get; set; }
        public int MaxTitleWords { get; set; }
        public int MaxContentWords { get; set; }
        public IEnumerable<SelectListItem> CategoryDropdownList { get; set; }
        public IEnumerable<SelectListItem> SubCategoryDropdownList { get; set; }
        public RatePrintClassifiedDisplay RatePrintClassifiedDisplay { get; set; }

        public string OriginalImageUrl { get; set; }

        public string FinalImageUrl { get; set; }
        public int EditionId { get; set; }
        public bool Personal { get; set; }
        public bool CompleteMatter { get; set; }
        public bool RequiredAdvertiser { get; set; }
        public decimal DefaultInchRate { get; set; }
        public Agency AgencyInfo { get; set; }
        public Advertiser AdvertiserInfo { get; set; }
        public string BookingNo { get; set; }


        [Display(Name = "Remarks")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }
    }


    public class ClasssifiedDisplayEstimatedCostingViewModel
    {
        public int ABPrintClassifiedDisplayId { get; set; }
        public string Title { get; set; }
        public string AdContent { get; set; }
        public IFormFile ImageContent { get; set; }
        public int ImageHeightInPixel { get; set; }
        public decimal EstimatedCosting { get; set; }
        public System.Drawing.Image FinalImage { get; set; }
        public decimal AdColumnInch { get; set; }
    }
}
