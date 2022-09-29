using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.DigitalAdUnitTypes
{
    public interface IDigitalAdUnitTypeService
    {
        Task<DigitalAdUnitType> GetDetailsById(int id);
        Task<IEnumerable<ConstantDropdownItem>> GetDigitalAdUnitTypeListForDropdown();
        Task<bool> Add(DigitalAdUnitType digitalAdUnitType);
        Task<bool> Update(DigitalAdUnitType digitalAdUnitType);
        Task<bool> Remove(DigitalAdUnitType digitalAdUnitType);
    }
}
