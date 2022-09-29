using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.Checkouts
{
    public class CheckoutProcessModel
    {
        public int CreatedBy { get; set; }
        public int ABPrintClassifiedTextId  { get; set; }
        public ABPrintClassifiedText ABPrintClassifiedText { get; set; }
        public List<ABPrintClassifiedTextDetail> ABPrintClassifiedTextDetailListing { get; set; }
    }
}
