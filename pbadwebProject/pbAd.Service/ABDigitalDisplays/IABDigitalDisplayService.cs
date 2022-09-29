using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABDigitalDisplays;
using pbAd.Data.DomainModels.ABPrintPrivateDisplays;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.ABDigitalDisplays
{
    public interface IABDigitalDisplayService
    {
        Task<IEnumerable<ABDigitalDisplay>> GetListByFilter(ABDigitalDisplaySearchFilter filter);
        Task<ABDigitalDisplay> GetDetails(int id, string bookingno, int bookingStep = 0, int createdBy = 0);
        Task<ABDigitalDisplayDetail> GetDigitalDisplayDetailById(int digitalDisplayId);
        Task<IEnumerable<ABDigitalDisplayDetail>> GetABDigitalDisplayDetailListing(int abDigitalDisplayId);
        Task<IEnumerable<DigitalDisplayMediaContent>> GetDigitalDisplayMediaContentListing(int digitalDisplayId);
        Task<int> GetTotalCountByPaymentMode(int paymentModeId);
        Task<bool> UpdatePaymentInfo(ABDigitalDisplay digitalDisplay);

        Task<ABDigitalDisplay> GetDetailsByIdAndBookingNo(int id, string bookingNo);
        Task<Response<ABDigitalDisplay>> Add(BookDigitalDisplayAdProcessModel model);
        Task<Response<CheckoutDigitalDisplayProcessModel>> AddABDigitalDisplayDetail(CheckoutDigitalDisplayProcessModel model);
        Task<bool> Update(ABDigitalDisplay abDigitalDisplay);
        Task<Response<ABDigitalDisplay>> EditDigitalDisplayBook(BookDigitalDisplayAdProcessModel model);
        Task<bool> CheckoutDigitalDisplay(ABDigitalDisplay abDigitalDisplay);
        Task<bool> Remove(ABDigitalDisplay abDigitalDisplay);
        Task<bool> UpdatePaymentMode(ABDigitalDisplay digitalDisplay);
        Task<Response<ABDigitalDisplay>> UpdateUploadLater(UploadLaterAdProcessModel model);
    }
}
