using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.Checkouts
{
    public class CheckoutClassfiedDisplayProcessModel
    {
        public int CreatedBy { get; set; }
        public int ABPrintClassifiedDisplayId  { get; set; }
        public ABPrintClassifiedDisplay ABPrintClassifiedDisplay { get; set; }
        public List<ABPrintClassifiedDisplayDetail> ABPrintClassifiedDisplayDetailListing { get; set; }
    }
}
