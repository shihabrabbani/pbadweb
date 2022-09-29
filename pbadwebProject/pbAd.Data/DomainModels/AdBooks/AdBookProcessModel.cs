using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.AdBooks
{
    public class AdBookProcessModel
    {
        public Advertiser Advertiser { get; set; }
        public ABPrintClassifiedText ABPrintClassifiedText { get; set; }
        public List<ABPrintClassifiedTextDetail> ABPrintClassifiedTextDetailListing { get; set; }
        public bool RequiredAdvertiser { get; set; }
    }
}
