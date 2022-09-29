using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.RatePrintClassifiedTexts
{
    public interface IRatePrintClassifiedTextService
    {
        Task<IEnumerable<RatePrintClassifiedText>> GetListByFilter(RatePrintClassifiedTextSearchFilter filter);
        Task<RatePrintClassifiedText> GetDetailsById(int id);
        Task<IEnumerable<RatePrintClassifiedText>> GetClassifiedTextRatesByEditionIds(List<int> editionIds);
        Task<RatePrintClassifiedText> GetDefaultRatePrintClassifiedText(int editionId);
        Task<bool> Add(RatePrintClassifiedText ratePrintClassifiedText);
        Task<bool> Update(RatePrintClassifiedText ratePrintClassifiedText);
        Task<bool> Remove(RatePrintClassifiedText ratePrintClassifiedText);
    }
}
