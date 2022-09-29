using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Data.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.RatePrintGovtAds
{
    public interface IRatePrintGovtAdService
    {
        Task<RatePrintGovtAd> GetDetailsById(int id);
        Task<RatePrintGovtAd> GetDefaultRatePrintGovtAd();
        Task<RatePrintGovtAd> GetRatePrintGovtAd(int editionId, int editionPageId);
        Task<View_RatePrintGovtAd> GetDefaultRatePrintGovtAd(int editionId, int editionPageNo);
        Task<IEnumerable<Edition>> GetAllEditions(EditionSearchFilter filter);
        Task<IEnumerable<View_RatePrintGovtAd>> GetGovtAdRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId);
        Task<bool> Add(RatePrintGovtAd ratePrintGovtAd);
        Task<bool> Update(RatePrintGovtAd ratePrintGovtAd);
        Task<bool> Remove(RatePrintGovtAd ratePrintGovtAd);
    }
}
