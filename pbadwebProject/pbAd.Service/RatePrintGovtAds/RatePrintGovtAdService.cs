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

namespace pbAd.Service.RatePrintGovtAds
{
    public class RatePrintGovtAdService : IRatePrintGovtAdService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public RatePrintGovtAdService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods

        public async Task<RatePrintGovtAd> GetDetailsById(int id)
        {
            var single = new RatePrintGovtAd();
            try
            {
                single = await db.RatePrintGovtAds.AsNoTracking().FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<RatePrintGovtAd> GetDefaultRatePrintGovtAd()
        {
            var single = new RatePrintGovtAd();
            try
            {
                single = await db.RatePrintGovtAds.AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<RatePrintGovtAd> GetRatePrintGovtAd(int editionId, int editionPageId)
        {
            var single = new RatePrintGovtAd();
            try
            {
                single = await db.RatePrintGovtAds.Where(f =>
                    f.EditionId == editionId && f.EditionPageId == editionPageId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<View_RatePrintGovtAd> GetDefaultRatePrintGovtAd(int editionId, int editionPageNo)
        {
            var single = new View_RatePrintGovtAd();
            try
            {
                single = await db.View_RatePrintGovtAds.Where(f =>
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

        public async Task<IEnumerable<Edition>> GetAllEditions(EditionSearchFilter filter)
        {
            var editionList = new List<Edition>();

            try
            {
                var editionIds = new List<int>();

                if (filter.IsColor)
                {
                    if (filter.Corporation)
                        editionIds = await db.View_RatePrintGovtAds.Where(f => f.EditionPageNo == filter.EditionPageNo && f.CorpColorRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                    else
                        editionIds = await db.View_RatePrintGovtAds.Where(f => f.EditionPageNo == filter.EditionPageNo && f.PerColumnInchColorRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                }
                else
                {
                    if (filter.Corporation)
                        editionIds = await db.View_RatePrintGovtAds.Where(f => f.EditionPageNo == filter.EditionPageNo && f.CorpBWRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
                    else
                        editionIds = await db.View_RatePrintGovtAds.Where(f => f.EditionPageNo == filter.EditionPageNo && f.PerColumnInchBWRate > 0).Select(s => s.EditionId).Distinct().ToListAsync();
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

        public async Task<IEnumerable<View_RatePrintGovtAd>> GetGovtAdRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId)
        {
            var listingList = new List<View_RatePrintGovtAd>();
            int editionPageNo = 0;
            var editionPage = await db.EditionPages.AsNoTracking().FirstOrDefaultAsync(f => f.EditionPageId == editionPageId);
            if (editionPage != null)
                editionPageNo = editionPage.EditionPageNo;

            try
            {
                IQueryable<View_RatePrintGovtAd> query = db.View_RatePrintGovtAds;
                listingList = await query.Where(f => f.EditionPageNo == editionPageNo && editionIds
                .Any(a => a == f.EditionId)).AsNoTracking().ToListAsync();

                return listingList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(RatePrintGovtAd ratePrintGovtAd)
        {
            try
            {
                await db.RatePrintGovtAds.AddAsync(ratePrintGovtAd);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(RatePrintGovtAd ratePrintGovtAd)
        {
            try
            {
                var upateRatePrintGovtAd = await db.RatePrintGovtAds.FirstOrDefaultAsync(d => d.AutoId == ratePrintGovtAd.AutoId);

                if (upateRatePrintGovtAd == null)
                    return false;

                upateRatePrintGovtAd.ModifiedBy = ratePrintGovtAd.ModifiedBy;
                upateRatePrintGovtAd.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(RatePrintGovtAd ratePrintGovtAd)
        {
            try
            {
                var removeRatePrintGovtAd = await db.RatePrintGovtAds.FirstOrDefaultAsync(d => d.AutoId == ratePrintGovtAd.AutoId);

                if (removeRatePrintGovtAd == null)
                    return false;

                db.RatePrintGovtAds.Remove(removeRatePrintGovtAd);
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
