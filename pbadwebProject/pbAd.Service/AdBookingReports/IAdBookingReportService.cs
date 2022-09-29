using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBookings;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Data.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.AdBookingReports
{
    public interface IAdBookingReportService
    {
        Task<IEnumerable<BookingOrdersModel>> GetListByFilter(AdBookingReportSearchFilter filter);

        Task<IEnumerable<UploadLaterOrdersModel>> GetUploadLatersByFilter(AdBookingReportSearchFilter filter);
    }
}
