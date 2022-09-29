using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.Editions
{
    public interface IEditionService
    {
        Task<IEnumerable<Edition>> GetListByFilter(EditionSearchFilter filter);
        Task<Edition> GetDetailsById(int id);
        Task<IEnumerable<Edition>> GetAllEditions();
        Task<IEnumerable<Edition>> GetAllEditionsByIds(List<int> editionIds);
        Task<bool> Add(Edition edition);
        Task<bool> Update(Edition edition);
        Task<bool> Remove(Edition edition);
    }
}
