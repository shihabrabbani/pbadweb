#region Usings
using pbAd.Core.Filters;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.OfferEditions
{
    public class OfferEditionService: IOfferEditionService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public OfferEditionService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods       

        public async Task<OfferEdition> GetDetailsById(int id)
        {
            var single = new OfferEdition();
            try
            {
                single = await db.OfferEditions.FirstOrDefaultAsync(d => d.EditionDateId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<OfferEdition> GetByNoofEdition(int noofEdition)
        {
            var single = new OfferEdition();
            try
            {
                single = await db.OfferEditions.OrderByDescending(o=>o.NoofEdition).FirstOrDefaultAsync(d => d.NoofEdition <= noofEdition);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<OfferEdition>> GetAllOfferEditions()
        {
            var offerDateList = new List<OfferEdition>();

            try
            {
                IQueryable<OfferEdition> query = db.OfferEditions;
                offerDateList = await query.ToListAsync();

                return offerDateList;
            }
            catch (Exception ex)
            {
                return null;
            }           
        }

        public async Task<bool> Add(OfferEdition offerDate)
        {
            try
            {
                await db.OfferEditions.AddAsync(offerDate);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(OfferEdition offerDate)
        {
            try
            {
                var upateOfferEdition = await db.OfferEditions.FirstOrDefaultAsync(d => d.EditionDateId == offerDate.EditionDateId);

                if (upateOfferEdition == null)
                    return false;
                
                upateOfferEdition.ModifiedBy = offerDate.ModifiedBy;
                upateOfferEdition.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(OfferEdition offerDate)
        {
            try
            {
                var removeOfferEdition = await db.OfferEditions.FirstOrDefaultAsync(d => d.EditionDateId == offerDate.EditionDateId);

                if (removeOfferEdition == null)
                    return false;

                db.OfferEditions.Remove(removeOfferEdition);
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
