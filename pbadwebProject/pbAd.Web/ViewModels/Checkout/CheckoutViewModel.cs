using pbAd.Data.Models;
using pbAd.Data.Views;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Checkout
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            this.EditionIds = new List<int>();
        }

        public IEnumerable<Edition> Editions { get; set; }
        public Edition DefaultEdition { get; set; }
        public int DefaultEditionId { get; set; }
        public string DefaultEditionName { get; set; }
        public List<int> EditionIds { get; set; }
        public ABPrintClassifiedText ABPrintClassifiedText { get; set; }
        public View_ABPrintClassifiedText ABPrintClassifiedTextView { get; set; }
        public string AdEnhancement { get; set; }
        public string BasedOfferDatesInString { get; set; }
        public int ABPrintClassifiedTextId { get; set; }
        public string BookingNo { get; set; }
        public string Remarks { get; set; }
        public int ManualDiscountPercentage { get; set; }
        public string AdContent { get; set; }
        public int PaymentModeId { get; set; }
        public bool NationalEdition { get; set; }
        public bool AcceptTermsAndConditions { get; set; }
        public string DefaultEditionToolTip { get; set; }
        public IEnumerable<EditionDistrict> EditionDistrictList { get; set; }
        public decimal Rate { get; set; }
        public string BillDate { get; set; }
    }
}
