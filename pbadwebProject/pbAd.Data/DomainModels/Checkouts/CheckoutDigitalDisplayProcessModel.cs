using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.Checkouts
{
    public class CheckoutDigitalDisplayProcessModel
    {
        public int CreatedBy { get; set; }
        public int ABDigitalDisplayId { get; set; }
        public ABDigitalDisplay ABDigitalDisplay { get; set; }
        public List<ABDigitalDisplayDetail> ABDigitalDisplayDetailListing { get; set; }
    }
}
