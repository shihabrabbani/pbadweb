using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Data.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.RatePrintSpotAds
{
    public interface IRatePrintSpotAdService
    {
        Task<RatePrintSpotAd> GetDetailsById(int id);
        Task<View_RatePrintSpotAd> GetDefaultRatePrintSpotAd(int editionId, int editionPageNo);
        Task<IEnumerable<RatePrintSpotAd>> GetRatePrintSpotAdsByEditionIds(List<int> editionIds, int editionPageId);
        Task<IEnumerable<View_RatePrintSpotAd>> GetSpotAdRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId);
        Task<bool> Add(RatePrintSpotAd ratePrintSpotAd);
        Task<bool> Update(RatePrintSpotAd ratePrintSpotAd);
        Task<bool> Remove(RatePrintSpotAd ratePrintSpotAd);
    }
}
