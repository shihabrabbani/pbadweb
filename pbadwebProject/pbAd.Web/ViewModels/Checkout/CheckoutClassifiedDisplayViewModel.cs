using pbAd.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Checkout
{
    public class CheckoutClassifiedDisplayViewModel
    {
        public CheckoutClassifiedDisplayViewModel()
        {
        }

        public IEnumerable<Edition> Editions { get; set; }
        public List<int> EditionIds { get; set; }
        public ABPrintClassifiedDisplay ABPrintClassifiedDisplay { get; set; }
        public string BasedOfferDatesInString { get; set; }
        public int ABPrintClassifiedDisplayId { get; set; }
        public string BookingNo { get; set; }        
        public string DefaultEditionName { get; set; }
        public int DefaultEditionId { get; set; }
        public int ManualDiscountPercentage { get; set; }
        public int PaymentModeId { get; set; }
        public bool AcceptTermsAndConditions { get; set; }
        public string Remarks { get; set; }
        public string FinalImageUrl { get; set; }
        public bool NationalEdition { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string AgencyName { get; set; }
        public int? AgencyId { get; set; }
        public string AdvertiserName { get; set; }
        public string AdvertiserMobileNo { get; set; }

        public decimal Rate { get; set; }
        public string BillDate { get; set; }
        public int? CollectorId { get; set; }
        public string DefaultEditionToolTip { get; set; }

        public IEnumerable<EditionDistrict> EditionDistrictList { get; set; }
    }    
}
