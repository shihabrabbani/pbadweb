using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.Checkouts
{
    public class CheckoutPrivateDisplayProcessModel
    {
        public int CreatedBy { get; set; }
        public int ABPrintPrivateDisplayId { get; set; }
        public ABPrintPrivateDisplay ABPrintPrivateDisplay { get; set; }
        public List<ABPrintPrivateDisplayDetail> ABPrintPrivateDisplayDetailListing { get; set; }
    }
}
