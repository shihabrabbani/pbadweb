using pbAd.Data.DomainModels.ABDigitalDisplays;
using pbAd.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.BookDigitalDisplayAds
{
    public class BookDigitalDisplayAdViewModel
    {
        [Display(Name = "Brand Auto Complete")]
        [Required(ErrorMessage = "{0} is Required")]
        public string BrandAutoComplete { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "{0} is Required")]
        public int BrandId { get; set; }

        [Display(Name = "Advertiser Name")]        
        public string AdvertiserName { get; set; }

        public bool RequiredAdvertiser { get; set; }

        public int AdvertiserId { get; set; }

        [Display(Name = "Advertiser Contact Number")]        
        public string AdvertiserContactNumber { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CategoryId { get; set; }

        [Display(Name = "Sub Category")]
        [Required(ErrorMessage = "{0} is Required")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Unit Type")]       
        public int? DigitalAdUnitTypeId { get; set; }

        [Display(Name = "Digital Page")]        
        public int? DigitalPageId { get; set; }

        [Display(Name = "Page Position")]       
        public int? DigitalPagePositionId { get; set; }

        [Display(Name = "Ad Qty")]       
        public int? AdQty { get; set; }

        [Display(Name = "Screen Type")]
        [Required(ErrorMessage = "{0} is Required")]
        public string ScreenType { get; set; }

        [Display(Name = "Estimated Total")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal EstimatedTotal { get; set; }

        public string ContentUrl { get; set; }

        [Display(Name = "Publish Date Start")]
        public DateTime? PublishDateStart { get; set; }

        [Display(Name = "Publish Time Start")]
        public string PublishTimeStart { get; set; }

        [Display(Name = "Publish Date End")]
        public DateTime? PublishDateEnd { get; set; }

        [Display(Name = "Publish Time End")]
        public string PublishTimeEnd { get; set; }


        //addtional
        [Display(Name = "Agency")]
        public int AgencyId { get; set; }

        [Display(Name = "Agency")]
        public string AgencyAutoComplete { get; set; }

        [Display(Name = "Upload Image")]
        public List<IFormFile> ImageContents { get; set; }

        public IEnumerable<SelectListItem> DigitalAdUnitTypeDropdownList { get; set; }
        public IEnumerable<SelectListItem> DigitalPageDropdownList { get; set; }
        //public IEnumerable<SelectListItem> DigitalPagePositionDropdownList { get; set; }


        public RateDigitalDisplay RateDigitalDisplay { get; set; }
        public RatePrintGovtAd RatePrintGovtAd { get; set; }
        public RatePrintSpotAd RatePrintSpotAd { get; set; }

        public decimal PerUnitRate { get; set; }

        public List<DigitalDisplayDetailModel> DigitalDisplayDetailList { get; set; }
        public bool UploadLater { get; set; }
        public int EditionId { get; set; }

        public string RemoveUploadedFiles { get; set; }
        public string RemoveExistingFiles { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        public string BookingNo { get; set; }
        public int ABDigitalDisplayId { get; set; }
        public bool IsFoundExistingFiles { get; set; }
        public IEnumerable<DigitalDisplayMediaContent> MediaContents { get; set; }
        public IEnumerable<ABDigitalDisplayDetail> DigitalDisplayDetailListing { get; set; }
        public string CheckScreenTypeDesktop { get; set; }
        public string CheckScreenTypeMobile { get; set; }
    }
}
