using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.OfferEditions
{
    public interface IOfferEditionService
    {
        Task<OfferEdition> GetDetailsById(int id);
        Task<OfferEdition> GetByNoofEdition(int noofEdition);
        Task<IEnumerable<OfferEdition>> GetAllOfferEditions();
        Task<bool> Add(OfferEdition offerEdition);
        Task<bool> Update(OfferEdition offerEdition);
        Task<bool> Remove(OfferEdition offerEdition);
    }
}
