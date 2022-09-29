using pbAd.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Checkout
{
    public class CheckoutDigitalDisplayViewModel
    {
        public CheckoutDigitalDisplayViewModel()
        {
        }

        public ABDigitalDisplay ABDigitalDisplay { get; set; }
        public IEnumerable<ABDigitalDisplayDetail> DigitalDisplayDetailListing { get; set; }
        public IEnumerable<DigitalDisplayMediaContent> MediaContentListing { get; set; }
        public string BasedOfferDatesInString { get; set; }
        public int ABDigitalDisplayId { get; set; }
        public string BookingNo { get; set; }
        public int PaymentModeId { get; set; }
        public bool AcceptTermsAndConditions { get; set; }
        public int ManualDiscountPercentage { get; set; }
        public string Remarks { get; set; }
        public string AdvertiserName { get; set; }
        public string AdvertiserMobile { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string AgencyName { get; set; }
        public int? CollectorId { get; set; }
        public int? AgencyId { get; set; }
        public DateTime BillDate { get; set; }

        public bool IsFixed { get; set; }
        public int FixedAmount { get; set; }
        public int MasterTableId { get; set; }
    }
}
