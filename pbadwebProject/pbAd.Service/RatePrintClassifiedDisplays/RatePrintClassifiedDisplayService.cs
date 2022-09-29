#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.RatePrintClassifiedDisplays
{
    public class RatePrintClassifiedDisplayService: IRatePrintClassifiedDisplayService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public RatePrintClassifiedDisplayService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods      

        public async Task<RatePrintClassifiedDisplay> GetDetailsById(int id)
        {
            var single = new RatePrintClassifiedDisplay();
            try
            {
                single = await db.RatePrintClassifiedDisplays.AsNoTracking().FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<RatePrintClassifiedDisplay> GetDefaultRatePrintClassifiedDisplay(int editionId)
        {
            var single = new RatePrintClassifiedDisplay();
            try
            {
                single = await db.RatePrintClassifiedDisplays.AsNoTracking().FirstOrDefaultAsync(f => f.EditionId == editionId);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<RatePrintClassifiedDisplay>> GetClassifiedDisplayRatesByEditionIds(List<int> editionIds)
        {
            var classifiedDisplayList = new List<RatePrintClassifiedDisplay>();

            try
            {
                IQueryable<RatePrintClassifiedDisplay> query = db.RatePrintClassifiedDisplays;
                classifiedDisplayList = await query.Where(f => editionIds.Any(a => a == f.EditionId))
                    .AsNoTracking()
                    .ToListAsync();

                return classifiedDisplayList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(RatePrintClassifiedDisplay ratePrintClassifiedDisplay)
        {
            try
            {
                await db.RatePrintClassifiedDisplays.AddAsync(ratePrintClassifiedDisplay);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(RatePrintClassifiedDisplay ratePrintClassifiedDisplay)
        {
            try
            {
                var upateRatePrintClassifiedDisplay = await db.RatePrintClassifiedDisplays.FirstOrDefaultAsync(d => d.AutoId == ratePrintClassifiedDisplay.AutoId);

                if (upateRatePrintClassifiedDisplay == null)
                    return false;
                
                upateRatePrintClassifiedDisplay.ModifiedBy = ratePrintClassifiedDisplay.ModifiedBy;
                upateRatePrintClassifiedDisplay.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(RatePrintClassifiedDisplay ratePrintClassifiedDisplay)
        {
            try
            {
                var removeRatePrintClassifiedDisplay = await db.RatePrintClassifiedDisplays.FirstOrDefaultAsync(d => d.AutoId == ratePrintClassifiedDisplay.AutoId);

                if (removeRatePrintClassifiedDisplay == null)
                    return false;

                db.RatePrintClassifiedDisplays.Remove(removeRatePrintClassifiedDisplay);
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
