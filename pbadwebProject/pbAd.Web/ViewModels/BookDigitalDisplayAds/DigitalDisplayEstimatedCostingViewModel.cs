using pbAd.Data.DomainModels.ABDigitalDisplays;
using System.Collections.Generic;

namespace pbAd.Web.ViewModels.BookDigitalDisplayAds
{
    public class DigitalDisplayEstimatedCostingViewModel
    {
        public List<DigitalDisplayDetailModel> DigitalDisplayDetailList { get; set; }
        public decimal EstimatedCosting { get; set; }
    }
}
