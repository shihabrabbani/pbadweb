using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.DigitalPages
{
    public interface IDigitalPageService
    {
        Task<DigitalPage> GetDetailsById(int id);
        Task<IEnumerable<ConstantDropdownItem>> GetDigitalPageListForDropdown();
        Task<bool> Add(DigitalPage digitalPage);
        Task<bool> Update(DigitalPage digitalPage);
        Task<bool> Remove(DigitalPage digitalPage);
    }
}
