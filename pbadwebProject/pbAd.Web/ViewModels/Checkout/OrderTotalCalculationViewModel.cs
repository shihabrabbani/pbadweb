using pbAd.Core.Utilities;
using pbAd.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Checkout
{
    public class OrderTotalCalculationViewModel
    {
        public OrderTotalCalculationViewModel()
        {
        }

        public bool NationalEdition { get; set; }
        public List<int> EditionIds { get; set; }
        public int MasterTableId { get; set; }
        public int DetailTableID { get; set; }
        public string BookingNo { get; set; }
        public decimal EstimatedTotalAmount { get; set; }
        public decimal EditionDiscountPercentage { get; set; }
        public decimal ManualDiscountPercentage { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TotalDiscountPercentages { get; set; }
        public int OfferDateId { get; set; }
        public int OfferEditionId { get; set; }
        //public bool IsPrivateDisplay { get;  set; }
        public string PrivateAdType { get; set; }
        public int EditionPageId { get; set; }
        public decimal Commission { get; set; }
        public decimal VATAmount { get; set; }
        public bool IsRateNotFound { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public decimal AdRate { get; set; }
        public int FixedAmount { get; set; }
        public bool IsFixed { get; set; }
        public int DateOfferPercentage { get; set; }
        public int EditionPageNo { get; set; }

        public List<int> ExceptEditionPageNoFor_Rajshahi_Rangpur => new List<int>
        {
            EditionPagesConstants.Page_5,
            EditionPagesConstants.Page_6,
            EditionPagesConstants.Page_7,
            EditionPagesConstants.Page_8
        };

        public int? AgencyId { get; set; }
    }

    public class EstimatedTotalCalculationViewModel
    {
        public EstimatedTotalCalculationViewModel()
        {

        }

        public int TotalWordCount { get; set; }
        public string AdContent { get; set; }
        public decimal EstimatedTotalAmount { get; set; }

        //classified text
        public bool IsBigBulletPointSingle { get; set; }
        public bool IsBigBulletPointDouble { get; set; }

        public bool IsBold { get; set; }
        public bool IsBoldinScreen { get; set; }
        public bool IsBoldScreenSingleBox { get; set; }
        public bool IsBoldScreenDoubleBox { get; set; }
    }
}
