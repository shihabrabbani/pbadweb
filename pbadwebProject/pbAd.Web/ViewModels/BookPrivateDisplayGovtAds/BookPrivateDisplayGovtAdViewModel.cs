using pbAd.Data.Models;
using pbAd.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.ViewModels.BookPrivateDisplayAds
{
    public class BookPrivateDisplayGovtAdViewModel
    {
        [Display(Name = "Advertiser Name")]
        public string AdvertiserName { get; set; }

        [Display(Name = "Advertiser Contact Number")]
        public string AdvertiserContactNumber { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CategoryId { get; set; }

        [Display(Name = "Brand Auto Complete")]
        [Required(ErrorMessage = "{0} is Required")]
        public string BrandAutoComplete { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "{0} is Required")]
        public int BrandId { get; set; }

        [Display(Name = "Edition Page")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EditionPageId { get; set; }


        [Display(Name = "Sub Category")]
        //[Required(ErrorMessage = "{0} is Required")]
        public int SubCategoryId { get; set; }


        [Display(Name = "Estimated Total")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal EstimatedTotal { get; set; }

        public string ContentUrl { get; set; }

        public string DatesBasedOffer { get; set; }
        public string[] DateBasedOfferList { get; set; }

        [Display(Name = "Agency")]
        public int AgencyId { get; set; }

        //addtional
        [Display(Name = "Agency")]
        public string AgencyAutoComplete { get; set; }

        [Display(Name = "Upload Image")]
        public List<IFormFile> ImageContents { get; set; }

        public string RemoveUploadedFiles { get; set; }
        public string RemoveExistingFiles { get; set; }

        public decimal PerColumnInchColorRate { get; set; }
        public decimal PerColumnInchBWRate { get; set; }
        public IEnumerable<SelectListItem> EditionPageDropdownList { get; set; }
        public IEnumerable<SelectListItem> ColumnSizeDropdownList { get; set; }
        public IEnumerable<SelectListItem> CategoryDropdownList { get; set; }
        public RatePrintPrivateDisplay RatePrintPrivateDisplay { get; set; }
        public RatePrintGovtAd RatePrintGovtAd { get; set; }
        public RatePrintSpotAd RatePrintSpotAd { get; set; }

        public bool IsColor { get; set; }
        public bool Personal { get; set; }
        public bool Corporation { get; set; }
        public bool RequiredAdvertiser { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Column Size")]
        [Required(ErrorMessage = "{0} is Required")]
        public int ColumnSize { get; set; }

        [Display(Name = "Inch Size")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal InchSize { get; set; }


        public bool IsSpot { get; set; }
        public bool IsGovt { get; set; }
        public int AdvertiserId { get; set; }
        public bool UploadLater { get; set; }
        public string BookingNo { get; set; }
        public bool IsFoundExistingFiles { get; set; }
        public int ABPrintPrivateDisplayId { get; set; }
        public IEnumerable<PrivateDisplayMediaContent> MediaContents { get; set; }
        public int EditionId { get; set; }
        public bool InsideDhaka { get; set; }
    }

    public class PrivateDisplayGovtEstimatedCostingViewModel
    {
        public int ColumnSize { get; set; }
        public decimal InchSize { get; set; }
        public bool IsColor { get; set; }
        public int EditionPageId { get; set; }
        public bool Corporation { get; set; }
        public decimal EstimatedCosting { get; set; }
        public decimal PerColumnInchColorRate { get; set; }
        public decimal PerColumnInchBWRate { get; set; }

       
        public decimal GovtAdRate { get; set; }
        public bool IsRateNotFound { get; set; }
        public int EditionId { get; set; }
        public bool IsNationalEditionRate { get; set; }
    }
}
