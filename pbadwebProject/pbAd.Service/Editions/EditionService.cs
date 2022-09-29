#region Usings
using pbAd.Core.Filters;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.Editions
{
    public class EditionService: IEditionService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public EditionService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Edition>> GetListByFilter(EditionSearchFilter filter)
        {
            var editionList = new List<Edition>();

            IQueryable<Edition> query = db.Editions;

            editionList = await query.ToListAsync();

            return editionList;
        }

        public async Task<Edition> GetDetailsById(int id)
        {
            var single = new Edition();
            try
            {
                single = await db.Editions.FirstOrDefaultAsync(d => d.EditionId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<Edition>> GetAllEditions()
        {
            var editionList = new List<Edition>();

            try
            {
                IQueryable<Edition> query = db.Editions;
                editionList = await query.ToListAsync();

                return editionList;
            }
            catch (Exception ex)
            {
                return null;
            }           
        }

        public async Task<IEnumerable<Edition>> GetAllEditionsByIds(List<int> editionIds)
        {
            var editionList = new List<Edition>();

            try
            {
                IQueryable<Edition> query = db.Editions;
                editionList = await query.Where(f=> editionIds.Any(a=>a==f.EditionId)).ToListAsync();

                return editionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(Edition edition)
        {
            try
            {
                await db.Editions.AddAsync(edition);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(Edition edition)
        {
            try
            {
                var upateEdition = await db.Editions.FirstOrDefaultAsync(d => d.EditionId == edition.EditionId);

                if (upateEdition == null)
                    return false;
                
                upateEdition.ModifiedBy = edition.ModifiedBy;
                upateEdition.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(Edition edition)
        {
            try
            {
                var removeEdition = await db.Editions.FirstOrDefaultAsync(d => d.EditionId == edition.EditionId);

                if (removeEdition == null)
                    return false;

                db.Editions.Remove(removeEdition);
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
