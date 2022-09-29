using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.Agencies
{
    public interface IAgencyService
    {
        Task<IEnumerable<Agency>> GetAgencyForAutoComplete(AgencySearchFilter filter);
        Task<Agency> GetDetailsById(int id);
        Task<Agency> GetDetailsByIdAndName(int id, string agentName);
        Task<IEnumerable<ConstantDropdownItem>> GetAgencyListForDropdown();
        Task<bool> Add(Agency agency);
        Task<bool> Update(Agency agency);
        Task<bool> Remove(Agency agency);
    }
}
