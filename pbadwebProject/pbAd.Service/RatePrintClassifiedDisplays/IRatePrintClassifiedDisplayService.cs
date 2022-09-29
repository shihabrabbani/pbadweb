using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Service.RatePrintClassifiedDisplays
{
    public interface IRatePrintClassifiedDisplayService
    {
        Task<RatePrintClassifiedDisplay> GetDetailsById(int id);
        Task<RatePrintClassifiedDisplay> GetDefaultRatePrintClassifiedDisplay(int editionId);
        Task<IEnumerable<RatePrintClassifiedDisplay>> GetClassifiedDisplayRatesByEditionIds(List<int> editionIds);
        Task<bool> Add(RatePrintClassifiedDisplay ratePrintClassifiedDisplay);
        Task<bool> Update(RatePrintClassifiedDisplay ratePrintClassifiedDisplay);
        Task<bool> Remove(RatePrintClassifiedDisplay ratePrintClassifiedDisplay);
    }
}
