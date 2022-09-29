using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.EditionPages
{
    public interface IEditionPageService
    {
        Task<IEnumerable<EditionPage>> GetListByFilter(EditionPageSearchFilter filter);
        Task<EditionPage> GetDetailsById(int id);
        Task<EditionPage> GetEdtionPageByEditionAndPageNo(int editionId, int pageNumber);
        Task<IEnumerable<EditionPage>> GetAllEditionPages();
        Task<IEnumerable<EditionPage>> GetEditionPagesByEditionAndPageNo(List<int> editionIds, int pageNo);
        Task<IEnumerable<EditionPage>> GetAllEditionPagesByIds(List<int> editionPageIds);
        Task<IEnumerable<ConstantDropdownItem>> GetEditionPageListForDropdown(int editionId);
        Task<bool> Add(EditionPage editionPage);
        Task<bool> Update(EditionPage editionPage);
        Task<bool> Remove(EditionPage editionPage);
    }
}
