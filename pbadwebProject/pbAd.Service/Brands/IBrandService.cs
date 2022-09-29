using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.Brands
{
    public interface IBrandService
    {
        Task<Brand> GetDetailsById(int id);
        Task<IEnumerable<Brand>> GetAllBrands();
        Task<IEnumerable<Brand>> GetBrandForAutoComplete(BrandSearchFilter filter);
        Task<IEnumerable<ConstantDropdownItem>> GetBrandListForDropdown();
        Task<BrandRelation> GetBrandWiseAdvirtiser(BrandSearchFilter filter);
        Task<bool> Add(Brand brand);
        Task<bool> Update(Brand brand);
        Task<bool> Remove(Brand brand);
    }
}
