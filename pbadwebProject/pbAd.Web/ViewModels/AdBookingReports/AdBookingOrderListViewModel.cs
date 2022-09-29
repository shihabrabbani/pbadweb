using pbAd.Core.Filters;
using pbAd.Data.DomainModels.AdBookings;
using System.Collections.Generic;

namespace pbAd.Web.ViewModels.AdBookingReports
{
    public class AdBookingOrderListViewModel
    {
        public IEnumerable<BookingOrdersModel> BookingOrderList { get; set; }
        public IEnumerable<UploadLaterOrdersModel> UploadLaterOrderList { get; set; }
        public AdBookingReportSearchFilter SearchFilter { get; set; }
    }
}
