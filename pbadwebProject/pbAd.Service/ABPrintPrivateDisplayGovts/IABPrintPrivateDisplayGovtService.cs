using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABPrintPrivateDisplays;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.ABPrintPrivateDisplayGovts
{
    public interface IABPrintPrivateDisplayGovtService
    {
        Task<IEnumerable<ABPrintPrivateDisplay>> GetListByFilter(ABPrintPrivateDisplaySearchFilter filter);
        Task<ABPrintPrivateDisplay> GetDetailsById(int id);
        Task<IEnumerable<PrivateDisplayMediaContent>> GetMediaContentsByPrivateDisplayId(int privateDisplayId);
        Task<ABPrintPrivateDisplay> GetDetail(int id, string bookingNo, int bookingStep = 0);
        Task<IEnumerable<PrivateDisplayMediaContent>> GetPrivateDisplayMediaContentListing(int abPrintPrivateDisplayId);
        Task<IEnumerable<ABPrintPrivateDisplayDetail>> GetABPrintPrivateDisplayDetailListing(int privateDisplayId);
        Task<Response<ABPrintPrivateDisplay>> BookPrivateDisplayAd(UploadLaterAdProcessModel model);
        Task<Response<CheckoutPrivateDisplayProcessModel>> CheckoutPrivateDisplayDetail(CheckoutPrivateDisplayProcessModel model);
        Task<Response<ABPrintPrivateDisplay>> EditPrivateDisplayBook(UploadLaterAdProcessModel model);
        Task<bool> Update(ABPrintPrivateDisplay privateDisplay);
        Task<bool> Remove(ABPrintPrivateDisplay privateDisplay);
    }
}
