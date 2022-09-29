using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.EditionDistricts
{
    public interface IEditionDistrictService
    {
        Task<EditionDistrict> GetDetailsById(int id);
        Task<IEnumerable<EditionDistrict>> GetAllEditionDistricts();
        Task<bool> Add(EditionDistrict brand);
        Task<bool> Update(EditionDistrict brand);
        Task<bool> Remove(EditionDistrict brand);
    }
}
