#region Usings
using pbAd.Core.Filters;
using pbAd.Data.Models;
using pbAd.Data.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.RatePrintSpotAds
{
    public class RatePrintSpotAdService: IRatePrintSpotAdService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public RatePrintSpotAdService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
       
        public async Task<RatePrintSpotAd> GetDetailsById(int id)
        {
            var single = new RatePrintSpotAd();
            try
            {
                single = await db.RatePrintSpotAds.FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<View_RatePrintSpotAd> GetDefaultRatePrintSpotAd(int editionId, int editionPageNo)
        {
            var single = new View_RatePrintSpotAd();
            try
            {
                single = await db.View_RatePrintSpotAds.Where(f =>
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

        public async Task<IEnumerable<RatePrintSpotAd>> GetRatePrintSpotAdsByEditionIds(List<int> editionIds,int editionPageId)
        {
            var listingList = new List<RatePrintSpotAd>();

            try
            {
                IQueryable<RatePrintSpotAd> query = db.RatePrintSpotAds;
                listingList = await query.Where(f =>f.EditionPageId == editionPageId && editionIds
                .Any(a => a == f.EditionId)).AsNoTracking().ToListAsync();

                return listingList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<View_RatePrintSpotAd>> GetSpotAdRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId)
        {
            var listingList = new List<View_RatePrintSpotAd>();
            int editionPageNo = 0;
            var editionPage = await db.EditionPages
                .AsNoTracking().FirstOrDefaultAsync(f => f.EditionPageId == editionPageId);
            if (editionPage != null)
                editionPageNo = editionPage.EditionPageNo;

            try
            {
                IQueryable<View_RatePrintSpotAd> query = db.View_RatePrintSpotAds;
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

        public async Task<bool> Add(RatePrintSpotAd ratePrintSpotAd)
        {
            try
            {
                await db.RatePrintSpotAds.AddAsync(ratePrintSpotAd);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(RatePrintSpotAd ratePrintSpotAd)
        {
            try
            {
                var upateRatePrintSpotAd = await db.RatePrintSpotAds.FirstOrDefaultAsync(d => d.AutoId == ratePrintSpotAd.AutoId);

                if (upateRatePrintSpotAd == null)
                    return false;
                
                upateRatePrintSpotAd.ModifiedBy = ratePrintSpotAd.ModifiedBy;
                upateRatePrintSpotAd.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(RatePrintSpotAd ratePrintSpotAd)
        {
            try
            {
                var removeRatePrintSpotAd = await db.RatePrintSpotAds.FirstOrDefaultAsync(d => d.AutoId == ratePrintSpotAd.AutoId);

                if (removeRatePrintSpotAd == null)
                    return false;

                db.RatePrintSpotAds.Remove(removeRatePrintSpotAd);
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
