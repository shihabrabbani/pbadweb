using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Core.Filters
{
    public class AdBookingReportSearchFilter : BaseSearchFilter
    {
        public DateTime? BillStartDate { get; set; }
        public DateTime? BillEndDate { get; set; }
    }
}
