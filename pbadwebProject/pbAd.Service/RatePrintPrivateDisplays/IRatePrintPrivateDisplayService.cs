using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Data.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.RatePrintPrivateDisplays
{
    public interface IRatePrintPrivateDisplayService
    {
        Task<RatePrintPrivateDisplay> GetDetailsById(int id);
        Task<View_RatePrintPrivateDisplay> GetDefaultRatePrintPrivateDisplay(int editionId, int editionPageNo);
        Task<IEnumerable<RatePrintPrivateDisplay>> GetPrivateDisplayRatesByEditionIds(List<int> editionIds, int editionPageId);
        Task<IEnumerable<View_RatePrintPrivateDisplay>> GetPrivateDisplayRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId);
        Task<IEnumerable<Edition>> GetAllEditions(EditionSearchFilter filter);
        Task<bool> Add(RatePrintPrivateDisplay ratePrintPrivateDisplay);
        Task<bool> Update(RatePrintPrivateDisplay ratePrintPrivateDisplay);
        Task<bool> Remove(RatePrintPrivateDisplay ratePrintPrivateDisplay);
    }
}
