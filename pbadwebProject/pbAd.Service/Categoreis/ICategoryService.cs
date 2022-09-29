using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetListByFilter(CategorySearchFilter filter);
        Task<Category> GetDetailsById(int id);
        Task<IEnumerable<ConstantDropdownItem>> GetCategoryListForDropdown(CategorySearchFilter filter);
        Task<IEnumerable<ConstantDropdownItem>> GetCategoryListForDropdownByBrandId(int brandId);
        Task<IEnumerable<ConstantDropdownItem>> GetCategoryListForDropdownDisplay();
        Task<bool> Add(Category categorySearchFilter);
        Task<bool> Update(Category categorySearchFilter);
        Task<bool> Remove(Category categorySearchFilter);
    }
}
