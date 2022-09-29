using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Data.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.ABPrintClassifiedTexts
{
    public interface IABPrintClassifiedTextService
    {
        Task<IEnumerable<ABPrintClassifiedText>> GetListByFilter(ABPrintClassifiedTextSearchFilter filter);
        Task<ABPrintClassifiedText> GetDetailsById(int id);
        Task<int> GetTotalCountByPaymentMode(int paymentModeId);
        Task<ABPrintClassifiedText> GetDetails(int id, string bookingNo, int bookingStep = 0, int createdBy = 0);
        Task<IEnumerable<ABPrintClassifiedTextDetail>> GetABPrintClassifiedTextDetailListing(int abPrintClassifiedTextId);
        Task<Response<ABPrintClassifiedText>> BookClassifiedTextAd(AdBookProcessModel model);
        Task<Response<ABPrintClassifiedText>> EditBookedClassifiedTextAd(AdBookProcessModel model);
        Task<Response<CheckoutProcessModel>> CheckoutClassifiedTextProcess(CheckoutProcessModel model);
        Task<bool> Update(ABPrintClassifiedText aBPrintClassifiedText);
        Task<bool> Remove(ABPrintClassifiedText aBPrintClassifiedText);
        Task<bool> UpdatePaymentInfo(ABPrintClassifiedText classifiedText);
        Task<bool> UpdatePaymentMode(ABPrintClassifiedText classifiedText);
        Task<string> CheckAgency(int id, string bookingno);
        Task<View_ABPrintClassifiedText> GetDetailsView(int id, string bookingNo, int bookingStep = 0);
    }
}
