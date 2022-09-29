using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.ABPrintPrivateDisplays
{
    public class UploadLaterAdProcessModel
    {
        public Advertiser Advertiser { get; set; }
        public bool RequiredAdvertiser { get; set; }
        public ABPrintPrivateDisplay ABPrintPrivateDisplay { get; set; }
        public ABDigitalDisplay ABDigitalDisplay { get; set; }
        public List<ABPrintPrivateDisplayDetail> ABPrintPrivateDisplayDetailListing { get; set; }
        public List<PrivateDisplayMediaContent> PrivateDisplayMediaContents { get; set; }
        public List<DigitalDisplayMediaContent> DigitalDisplayMediaContents { get; set; }
        public string RemoveExistingFiles { get; set; }
    }
}
