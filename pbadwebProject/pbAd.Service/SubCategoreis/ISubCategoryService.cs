using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.SubCategories
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategory>> GetListByFilter(SubCategorySearchFilter filter);
        Task<SubCategory> GetDetailsById(int id);
        Task<IEnumerable<SubCategory>> GetSubCategoryByCategory(int categoryId);
        Task<IEnumerable<ConstantDropdownItem>> GetSubCategoryListForDropdown(int categoryId);
        Task<bool> Add(SubCategory subCategory);
        Task<bool> Update(SubCategory subCategory);
        Task<bool> Remove(SubCategory subCategory);
    }
}
