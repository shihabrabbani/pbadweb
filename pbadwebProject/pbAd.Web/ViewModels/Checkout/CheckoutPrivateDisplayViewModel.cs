using pbAd.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Checkout
{
    public class CheckoutPrivateDisplayViewModel
    { 
        public CheckoutPrivateDisplayViewModel()
        {
        }

        public IEnumerable<Edition> Editions { get; set; }
        public List<int> EditionIds { get; set; }
        public ABPrintPrivateDisplay ABPrintPrivateDisplay { get; set; }
        public ABPrintPrivateDisplayDetail PrivateDisplayDetail { get; set; }
        public string BasedOfferDatesInString { get; set; }
        public int ABPrintPrivateDisplayId { get; set; }
        public string BookingNo { get; set; }
        public string DefaultEditionName { get; set; }
        public int DefaultEditionId { get; set; }
        public int ManualDiscountPercentage { get; set; } 
        public bool IsFixed { get; set; }
        public int FixedAmount { get; set; }
        public int PaymentModeId { get; set; }
        public bool AcceptTermsAndConditions { get; set; }
        public string Remarks { get; set; }
        public IEnumerable<EditionDistrict> EditionDistrictList { get; set; }
        public IEnumerable<PrivateDisplayMediaContent> DisplayMediaContentListing { get; set; }
        public int EditionPageId { get; set; }
        public string EditionPageName { get; set; }
        public bool NationalEdition { get; set; }
        public int EditionPageNo { get; set; }

        public string CategoryName { get; set; }
        public string AgencyName { get; set; }
        public string BrandName { get; set; }
        public string AdvertiserName { get; set; }
        public string AdvertiserMobile { get; set; }
        public string BillDate { get; set; }
        public int? AgencyId { get; set; }
        public int? CollectorId { get; set; }
        public string PrivateAdType { get; set; }
    }    
}
