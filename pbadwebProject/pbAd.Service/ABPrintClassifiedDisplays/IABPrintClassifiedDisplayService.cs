using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.DomainModels.BookClassifiedDisplayAds;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.ABPrintClassifiedDisplays
{
    public interface IABPrintClassifiedDisplayService
    {
        Task<IEnumerable<ABPrintClassifiedDisplay>> GetListByFilter(ABPrintClassifiedDisplaySearchFilter filter);
        Task<ABPrintClassifiedDisplay> GetDetailsByIdAndBookingNo(int id, string bookingNo, int bookingStep = 0);
        Task<int> GetTotalCountByPaymentMode(int paymentModeId);
        Task<ABPrintClassifiedDisplay> GetDetails(int id, string bookingNo, int bookingStep = 0, int createdBy = 0);
        Task<ABPrintClassifiedDisplayDetail> GetABPrintClassifiedDisplayDetail(int abPrintClassifiedDisplayId);
        Task<IEnumerable<ABPrintClassifiedDisplayDetail>> GetABPrintClassifiedDisplayDetailListing(int abPrintClassifiedDisplayId);
        Task<Response<ABPrintClassifiedDisplay>> BookClassifiedDisplayAd(BookClassifiedDisplayAdProcessModel model);
        Task<Response<ABPrintClassifiedDisplay>> EditBookedClassifiedDisplayAd(BookClassifiedDisplayAdProcessModel model);
        Task<Response<CheckoutClassfiedDisplayProcessModel>> CheckoutClassifiedDisplayProcess(CheckoutClassfiedDisplayProcessModel model);
        Task<bool> Update(ABPrintClassifiedDisplay abPrintClassifiedDisplay);
        Task<bool> Remove(ABPrintClassifiedDisplay abPrintClassifiedDisplay);
        Task<bool> UpdatePaymentInfo(ABPrintClassifiedDisplay classifiedDisplay);
        Task<bool> UpdatePaymentMode(ABPrintClassifiedDisplay aBPrintClassifiedDisplay);
    }
}
