#region Usings
using pbAd.Core.Filters;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.RatePrintClassifiedTexts
{
    public class RatePrintClassifiedTextService: IRatePrintClassifiedTextService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public RatePrintClassifiedTextService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<RatePrintClassifiedText>> GetListByFilter(RatePrintClassifiedTextSearchFilter filter)
        {
            var ratePrintClassifiedTextList = new List<RatePrintClassifiedText>();

            IQueryable<RatePrintClassifiedText> query = db.RatePrintClassifiedTexts;

            ratePrintClassifiedTextList = await query.ToListAsync();

            return ratePrintClassifiedTextList;
        }

        public async Task<RatePrintClassifiedText> GetDetailsById(int id)
        {
            var single = new RatePrintClassifiedText();
            try
            {
                single = await db.RatePrintClassifiedTexts.AsNoTracking().FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }
        public async Task<IEnumerable<RatePrintClassifiedText>> GetClassifiedTextRatesByEditionIds(List<int> editionIds)
        {
            var classifiedTextList = new List<RatePrintClassifiedText>();

            try
            {
                if(editionIds==null || !editionIds.Any())
                    return new List<RatePrintClassifiedText>();

                IQueryable<RatePrintClassifiedText> query = db.RatePrintClassifiedTexts;
                classifiedTextList = await query.Where(f => editionIds.Any(a => a == f.EditionId))
                    .AsNoTracking().ToListAsync();

                return classifiedTextList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<RatePrintClassifiedText> GetDefaultRatePrintClassifiedText(int editionId)
        {
            var single = new RatePrintClassifiedText();
            try
            {
                single = await db.RatePrintClassifiedTexts.Where(f=>f.EditionId== editionId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<bool> Add(RatePrintClassifiedText ratePrintClassifiedText)
        {
            try
            {
                await db.RatePrintClassifiedTexts.AddAsync(ratePrintClassifiedText);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(RatePrintClassifiedText ratePrintClassifiedText)
        {
            try
            {
                var upateRatePrintClassifiedText = await db.RatePrintClassifiedTexts.FirstOrDefaultAsync(d => d.AutoId == ratePrintClassifiedText.AutoId);

                if (upateRatePrintClassifiedText == null)
                    return false;
                
                upateRatePrintClassifiedText.ModifiedBy = ratePrintClassifiedText.ModifiedBy;
                upateRatePrintClassifiedText.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(RatePrintClassifiedText ratePrintClassifiedText)
        {
            try
            {
                var removeRatePrintClassifiedText = await db.RatePrintClassifiedTexts.FirstOrDefaultAsync(d => d.AutoId == ratePrintClassifiedText.AutoId);

                if (removeRatePrintClassifiedText == null)
                    return false;

                db.RatePrintClassifiedTexts.Remove(removeRatePrintClassifiedText);
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
