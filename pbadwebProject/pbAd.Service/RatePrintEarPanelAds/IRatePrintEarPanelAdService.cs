using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Data.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.RatePrintEarPanelAds
{
    public interface IRatePrintEarPanelAdService
    {
        Task<IEnumerable<RatePrintEarPanelAd>> GetListByFilter(RatePrintEarPanelAdSearchFilter filter);
        Task<RatePrintEarPanelAd> GetDetailsById(int id);
        Task<IEnumerable<RatePrintEarPanelAd>> GetRatePrintEarPanelAdsByEditionIds(List<int> editionIds);
        Task<View_RatePrintEarPanelAd> GetDefaultRatePrintEarPanelAd(int editionId, int editionPageNo);
        Task<IEnumerable<View_RatePrintEarPanelAd>> GetEARPanelAdRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId);
        Task<bool> Add(RatePrintEarPanelAd ratePrintEarPanelAd);
        Task<bool> Update(RatePrintEarPanelAd ratePrintEarPanelAd);
        Task<bool> Remove(RatePrintEarPanelAd ratePrintEarPanelAd);
    }
}
