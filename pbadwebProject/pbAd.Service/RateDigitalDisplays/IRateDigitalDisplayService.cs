using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.RateDigitalDisplays
{
    public interface IRateDigitalDisplayService
    {
        Task<RateDigitalDisplay> GetDetailsById(int id);
        Task<RateDigitalDisplay> GetDefaultRateDigitalDisplay();
        Task<IEnumerable<RateDigitalDisplay>> GetDigitalDisplayRates();
        Task<RateDigitalDisplay> GetByUnitTypePageAndPosition(int digitalAdUnitTypeId, int digitalPageId, int digitalPagePositionId);
        Task<bool> Add(RateDigitalDisplay rateDigitalDisplay);
        Task<bool> Update(RateDigitalDisplay rateDigitalDisplay);
        Task<bool> Remove(RateDigitalDisplay rateDigitalDisplay);
    }
}
