using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.ABDigitalDisplays
{
    public class BookDigitalDisplayAdProcessModel
    {
        public Advertiser Advertiser { get; set; }
        public ABDigitalDisplay ABDigitalDisplay { get; set; }
        public List<DigitalDisplayMediaContent> DigitalDisplayMediaContents { get; set; }
        public List<ABDigitalDisplayDetail> ABDigitalDisplayDetailListing { get; set; }
        public bool RequiredAdvertiser { get; set; }
        public string RemoveExistingFiles { get; set; }
        public string ScreenType { get; set; }
    }
}
