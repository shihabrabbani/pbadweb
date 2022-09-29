using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Core.Filters
{
    public class AdvertiserSearchFilter : BaseSearchFilter
    {
        public int? AdvertiserId { get; set; }
        public string MobileNo { get; set; }
        public int? AdvertiserType { get; set; }
    }
}
