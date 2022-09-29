using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.DefaultDiscounts
{
    public interface IDefaultDiscountService
    {
        Task<DefaultDiscount> GetDetailsById(int id);
        Task<DefaultDiscount> GetDetailsByAdTypeAndPaymentMode(string adType, int paymentModeId);
        Task<IEnumerable<DefaultDiscount>> GetAllDefaultDiscounts();
        Task<IEnumerable<ConstantDropdownItem>> GetDefaultDiscountListForDropdown();
        Task<bool> Add(DefaultDiscount defaultDiscount);
        Task<bool> Update(DefaultDiscount defaultDiscount);
        Task<bool> Remove(DefaultDiscount defaultDiscount);
    }
}
