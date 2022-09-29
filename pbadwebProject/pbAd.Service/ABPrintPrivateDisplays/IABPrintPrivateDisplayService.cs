using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABPrintPrivateDisplays;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.ABPrintPrivateDisplays
{
    public interface IABPrintPrivateDisplayService
    {
        Task<IEnumerable<ABPrintPrivateDisplay>> GetListByFilter(ABPrintPrivateDisplaySearchFilter filter);
        Task<ABPrintPrivateDisplay> GetDetailsById(int id);
        Task<int> GetTotalCountByPaymentMode(int paymentModeId);
        Task<ABPrintPrivateDisplay> GetDetailsByIdAndBookingNo(int id, string bookingNo);
        Task<IEnumerable<PrivateDisplayMediaContent>> GetMediaContentsByPrivateDisplayId(int privateDisplayId);
        Task<ABPrintPrivateDisplay> GetDetails(int id, string bookingNo, int bookingStep = 0, int createdBy = 0);
        Task<IEnumerable<PrivateDisplayMediaContent>> GetPrivateDisplayMediaContentListing(int abPrintPrivateDisplayId);
        Task<IEnumerable<ABPrintPrivateDisplayDetail>> GetABPrintPrivateDisplayDetailListing(int privateDisplayId);
        Task<Response<ABPrintPrivateDisplay>> BookPrivateDisplayAd(UploadLaterAdProcessModel model);
        Task<Response<CheckoutPrivateDisplayProcessModel>> CheckoutPrivateDisplayDetail(CheckoutPrivateDisplayProcessModel model);
        Task<Response<ABPrintPrivateDisplay>> EditPrivateDisplayBook(UploadLaterAdProcessModel model);
        Task<bool> Update(ABPrintPrivateDisplay privateDisplay);
        Task<bool> Remove(ABPrintPrivateDisplay privateDisplay);
        Task<bool> UpdatePaymentInfo(ABPrintPrivateDisplay privateDisplay);
        Task<bool> UpdatePaymentMode(ABPrintPrivateDisplay aBPrintPrivateDisplay);
        Task<Response<ABPrintPrivateDisplay>> UpdateUploadLater(UploadLaterAdProcessModel model);
    }
}
