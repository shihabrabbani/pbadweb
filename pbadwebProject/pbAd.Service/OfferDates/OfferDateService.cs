#region Usings
using pbAd.Core.Filters;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.OfferDates
{
    public class OfferDateService: IOfferDateService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public OfferDateService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods       

        public async Task<OfferDate> GetDetailsById(int id)
        {
            var single = new OfferDate();
            try
            {
                single = await db.OfferDates.FirstOrDefaultAsync(d => d.OfferDateId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<OfferDate> GetByDaysRangeAndNoofTime(List<DateTime> datesBasedOffer, int noofTime)
        {
            var single = new OfferDate();
            try
            {
                DateTime minDate = datesBasedOffer.OrderBy(f => f).FirstOrDefault();
                DateTime maxDate = datesBasedOffer.OrderByDescending(f => f).FirstOrDefault();
                
                foreach (var date in datesBasedOffer.Where(f=>f<= maxDate).OrderByDescending(f=>f))
                {
                    var daysRange = (int)(maxDate - minDate).TotalDays + 1;

                    single = await db.OfferDates.Where(d => d.IsActive 
                        && (d.NoofTimeFrom <= noofTime && d.NoofTimeTo >= noofTime)
                        && (d.DaysRangeFrom <= daysRange && d.DaysRangeTo >= daysRange))
                    .OrderByDescending(f=>f.DiscountPercentage).FirstOrDefaultAsync();

                    if (single != null)
                        break;

                    maxDate = datesBasedOffer.Where(w=>w < date).OrderByDescending(f => f).FirstOrDefault();
                }                
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<OfferDate>> GetAllOfferDates()
        {
            var offerDateList = new List<OfferDate>();

            try
            {
                IQueryable<OfferDate> query = db.OfferDates;
                offerDateList = await query.ToListAsync();

                return offerDateList;
            }
            catch (Exception ex)
            {
                return null;
            }           
        }

        public async Task<bool> Add(OfferDate offerDate)
        {
            try
            {
                await db.OfferDates.AddAsync(offerDate);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(OfferDate offerDate)
        {
            try
            {
                var upateOfferDate = await db.OfferDates.FirstOrDefaultAsync(d => d.OfferDateId == offerDate.OfferDateId);

                if (upateOfferDate == null)
                    return false;
                
                upateOfferDate.ModifiedBy = offerDate.ModifiedBy;
                upateOfferDate.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(OfferDate offerDate)
        {
            try
            {
                var removeOfferDate = await db.OfferDates.FirstOrDefaultAsync(d => d.OfferDateId == offerDate.OfferDateId);

                if (removeOfferDate == null)
                    return false;

                db.OfferDates.Remove(removeOfferDate);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
