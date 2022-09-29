#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Data.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.RatePrintPrivateDisplays
{
    public class RatePrintPrivateDisplayService : IRatePrintPrivateDisplayService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public RatePrintPrivateDisplayService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods

        public async Task<RatePrintPrivateDisplay> GetDetailsById(int id)
        {
            var single = new RatePrintPrivateDisplay();
            try
            {
                single = await db.RatePrintPrivateDisplays
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }


        public async Task<View_RatePrintPrivateDisplay> GetDefaultRatePrintPrivateDisplay(int editionId, int editionPageNo)
        {
            var single = new View_RatePrintPrivateDisplay();
            try
            {
                single = await db.View_RatePrintPrivateDisplays                    
                    .Where(f =>
                    f.EditionId == editionId && f.EditionPageNo == editionPageNo)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<RatePrintPrivateDisplay>> GetPrivateDisplayRatesByEditionIds(List<int> editionIds, int editionPageId)
        {
            var listingList = new List<RatePrintPrivateDisplay>();

            try
            {
                IQueryable<RatePrintPrivateDisplay> query = db.RatePrintPrivateDisplays;
                listingList = await query.Where(f => f.EditionPageId == editionPageId && editionIds.Any(a => a == f.EditionId))
                    .AsNoTracking()
                    .ToListAsync();

                return listingList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<View_RatePrintPrivateDisplay>> GetPrivateDisplayRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId)
        {
            var listingList = new List<View_RatePrintPrivateDisplay>();
            int editionPageNo = 0;
            var editionPage = await db.EditionPages
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.EditionPageId == editionPageId);
            if (editionPage != null)
                editionPageNo = editionPage.EditionPageNo;

            try
            {
                IQueryable<View_RatePrintPrivateDisplay> query = db.View_RatePrintPrivateDisplays;
                listingList = await query.Where(f => f.EditionPageNo == editionPageNo && editionIds.Any(a => a == f.EditionId))
                    .AsNoTracking()
                    .ToListAsync();

                return listingList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Edition>> GetAllEditions(EditionSearchFilter filter)
        {
            var editionList = new List<Edition>();

            try
            {
                var editionIds = new List<int>();

                if (filter.PrivateAdType == PrivateAdTypesConstants.Private)
                {
                    if (filter.IsColor)
                        editionIds = await db.View_RatePrintPrivateDisplays.Where(f => f.EditionPageNo == filter.EditionPageNo && f.PerColumnInchColorRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                    else
                        editionIds = await db.View_RatePrintPrivateDisplays.Where(f => f.EditionPageNo == filter.EditionPageNo && f.PerColumnInchBWRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                }
                else if (filter.PrivateAdType == PrivateAdTypesConstants.Spot)
                {
                    if (filter.IsColor)
                        editionIds = await db.View_RatePrintSpotAds.Where(f => f.EditionPageNo == filter.EditionPageNo && f.PerColumnInchColorRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                    else
                        editionIds = await db.View_RatePrintSpotAds.Where(f => f.EditionPageNo == filter.EditionPageNo && f.PerColumnInchBWRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                }
                else if (filter.PrivateAdType == PrivateAdTypesConstants.EARPanel)
                {
                    editionIds = await db.View_RatePrintEarPanelAds.Where(f => f.EditionPageNo == filter.EditionPageNo && f.Rate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                }                

                IQueryable<Edition> query = db.Editions;
                editionList = await query.Where(f => editionIds.Any(a => a == f.EditionId)).AsNoTracking().ToListAsync();

                return editionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(RatePrintPrivateDisplay ratePrintPrivateDisplay)
        {
            try
            {
                await db.RatePrintPrivateDisplays.AddAsync(ratePrintPrivateDisplay);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(RatePrintPrivateDisplay ratePrintPrivateDisplay)
        {
            try
            {
                var upateRatePrintPrivateDisplay = await db.RatePrintPrivateDisplays.FirstOrDefaultAsync(d => d.AutoId == ratePrintPrivateDisplay.AutoId);

                if (upateRatePrintPrivateDisplay == null)
                    return false;

                upateRatePrintPrivateDisplay.ModifiedBy = ratePrintPrivateDisplay.ModifiedBy;
                upateRatePrintPrivateDisplay.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(RatePrintPrivateDisplay ratePrintPrivateDisplay)
        {
            try
            {
                var removeRatePrintPrivateDisplay = await db.RatePrintPrivateDisplays.FirstOrDefaultAsync(d => d.AutoId == ratePrintPrivateDisplay.AutoId);

                if (removeRatePrintPrivateDisplay == null)
                    return false;

                db.RatePrintPrivateDisplays.Remove(removeRatePrintPrivateDisplay);
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
