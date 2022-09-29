using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.BookClassifiedDisplayAds
{
    public class BookClassifiedDisplayAdProcessModel
    {
        public Advertiser Advertiser { get; set; }
        public ABPrintClassifiedDisplay ABPrintClassifiedDisplay { get; set; }
        public List<ABPrintClassifiedDisplayDetail> ABPrintClassifiedDisplayDetailListing { get; set; }
        public int ABPrintClassifiedDisplayId { get; set; }
        public bool RequiredAdvertiser { get; set; }
    }
}
