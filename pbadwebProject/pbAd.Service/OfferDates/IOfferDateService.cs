using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.OfferDates
{
    public interface IOfferDateService
    {
        Task<OfferDate> GetDetailsById(int id);
        Task<OfferDate> GetByDaysRangeAndNoofTime(List<DateTime> datesBasedOffer, int noofTime);
        Task<IEnumerable<OfferDate>> GetAllOfferDates();
        Task<bool> Add(OfferDate offerDate);
        Task<bool> Update(OfferDate offerDate);
        Task<bool> Remove(OfferDate offerDate);
    }
}
