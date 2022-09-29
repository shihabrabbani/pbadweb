using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.DigitalPagePositions
{
    public interface IDigitalPagePositionService
    {
        Task<DigitalPagePosition> GetDetailsById(int id);
        Task<IEnumerable<ConstantDropdownItem>> GetDigitalPagePositionListForDropdown(int? pageId);
        Task<bool> Add(DigitalPagePosition digitalPagePosition);
        Task<bool> Update(DigitalPagePosition digitalPagePosition);
        Task<bool> Remove(DigitalPagePosition digitalPagePosition);
    }
}
