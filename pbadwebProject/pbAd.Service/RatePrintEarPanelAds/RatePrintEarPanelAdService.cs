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

namespace pbAd.Service.RatePrintEarPanelAds
{
    public class RatePrintEarPanelAdService: IRatePrintEarPanelAdService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public RatePrintEarPanelAdService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<RatePrintEarPanelAd>> GetListByFilter(RatePrintEarPanelAdSearchFilter filter)
        {
            var ratePrintEarPanelAdList = new List<RatePrintEarPanelAd>();

            IQueryable<RatePrintEarPanelAd> query = db.RatePrintEarPanelAds;

            ratePrintEarPanelAdList = await query.AsNoTracking().ToListAsync();

            return ratePrintEarPanelAdList;
        }

        public async Task<RatePrintEarPanelAd> GetDetailsById(int id)
        {
            var single = new RatePrintEarPanelAd();
            try
            {
                single = await db.RatePrintEarPanelAds.AsNoTracking().FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }
        public async Task<IEnumerable<RatePrintEarPanelAd>> GetRatePrintEarPanelAdsByEditionIds(List<int> editionIds)
        {
            var classifiedTextList = new List<RatePrintEarPanelAd>();

            try
            {
                if(editionIds==null || !editionIds.Any())
                    return new List<RatePrintEarPanelAd>();

                IQueryable<RatePrintEarPanelAd> query = db.RatePrintEarPanelAds;
                classifiedTextList = await query.Where(f => editionIds.Any(a => a == f.EditionId)).ToListAsync();

                return classifiedTextList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<View_RatePrintEarPanelAd> GetDefaultRatePrintEarPanelAd(int editionId, int editionPageNo)
        {
            var single = new View_RatePrintEarPanelAd();
            try
            {
                single = await db.View_RatePrintEarPanelAds.Where(f =>
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

        public async Task<IEnumerable<View_RatePrintEarPanelAd>> GetEARPanelAdRatesByEditionIdsAndEditionPage(List<int> editionIds, int editionPageId)
        {
            var listingList = new List<View_RatePrintEarPanelAd>();
            int editionPageNo = 0;
            var editionPage = await db.EditionPages.FirstOrDefaultAsync(f => f.EditionPageId == editionPageId);
            if (editionPage != null)
                editionPageNo = editionPage.EditionPageNo;

            try
            {
                IQueryable<View_RatePrintEarPanelAd> query = db.View_RatePrintEarPanelAds;
                listingList = await query.Where(f => f.EditionPageNo == editionPageNo && editionIds
                    .Any(a => a == f.EditionId))
                    .AsNoTracking()
                    .ToListAsync();

                return listingList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(RatePrintEarPanelAd ratePrintEarPanelAd)
        {
            try
            {
                await db.RatePrintEarPanelAds.AddAsync(ratePrintEarPanelAd);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(RatePrintEarPanelAd ratePrintEarPanelAd)
        {
            try
            {
                var upateRatePrintEarPanelAd = await db.RatePrintEarPanelAds.FirstOrDefaultAsync(d => d.AutoId == ratePrintEarPanelAd.AutoId);

                if (upateRatePrintEarPanelAd == null)
                    return false;
                
                upateRatePrintEarPanelAd.ModifiedBy = ratePrintEarPanelAd.ModifiedBy;
                upateRatePrintEarPanelAd.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(RatePrintEarPanelAd ratePrintEarPanelAd)
        {
            try
            {
                var removeRatePrintEarPanelAd = await db.RatePrintEarPanelAds.FirstOrDefaultAsync(d => d.AutoId == ratePrintEarPanelAd.AutoId);

                if (removeRatePrintEarPanelAd == null)
                    return false;

                db.RatePrintEarPanelAds.Remove(removeRatePrintEarPanelAd);
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
