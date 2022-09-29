#region Usings
using pbAd.Core.Filters;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.RateDigitalDisplays
{
    public class RateDigitalDisplayService: IRateDigitalDisplayService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public RateDigitalDisplayService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
       
        public async Task<RateDigitalDisplay> GetDetailsById(int id)
        {
            var single = new RateDigitalDisplay();
            try
            {
                single = await db.RateDigitalDisplays.AsNoTracking().FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<RateDigitalDisplay>> GetDigitalDisplayRates()
        {
            var filteredList = new List<RateDigitalDisplay>();
            try
            {
                filteredList = await db.RateDigitalDisplays.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<RateDigitalDisplay>();
            }
            return filteredList;
        }

        public async Task<RateDigitalDisplay> GetDefaultRateDigitalDisplay()
        {
            var single = new RateDigitalDisplay();
            try
            {
                single = await db.RateDigitalDisplays.AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<RateDigitalDisplay> GetByUnitTypePageAndPosition(int digitalAdUnitTypeId,int digitalPageId, int digitalPagePositionId)
        {
            var single = new RateDigitalDisplay();
            try
            {
                single = await db.RateDigitalDisplays
                                .AsNoTracking()
                                .FirstOrDefaultAsync(f=>f.DigitalAdUnitTypeId==digitalAdUnitTypeId 
                                        && f.DigitalPageId==digitalPageId
                                        && f.DigitalPagePositionId==digitalPagePositionId);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<bool> Add(RateDigitalDisplay rateDigitalDisplay)
        {
            try
            {
                await db.RateDigitalDisplays.AddAsync(rateDigitalDisplay);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(RateDigitalDisplay rateDigitalDisplay)
        {
            try
            {
                var upateRateDigitalDisplay = await db.RateDigitalDisplays.FirstOrDefaultAsync(d => d.AutoId == rateDigitalDisplay.AutoId);

                if (upateRateDigitalDisplay == null)
                    return false;
                
                upateRateDigitalDisplay.ModifiedBy = rateDigitalDisplay.ModifiedBy;
                upateRateDigitalDisplay.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(RateDigitalDisplay rateDigitalDisplay)
        {
            try
            {
                var removeRateDigitalDisplay = await db.RateDigitalDisplays.FirstOrDefaultAsync(d => d.AutoId == rateDigitalDisplay.AutoId);

                if (removeRateDigitalDisplay == null)
                    return false;

                db.RateDigitalDisplays.Remove(removeRateDigitalDisplay);
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
