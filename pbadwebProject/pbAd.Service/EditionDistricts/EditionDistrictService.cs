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

namespace pbAd.Service.EditionDistricts
{
    public class EditionDistrictService : IEditionDistrictService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public EditionDistrictService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods

        public async Task<EditionDistrict> GetDetailsById(int id)
        {
            var single = new EditionDistrict();
            try
            {
                single = await db.EditionDistricts.FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<EditionDistrict>> GetAllEditionDistricts()
        {
            var editionDistrictList = new List<EditionDistrict>();

            try
            {
                IQueryable<EditionDistrict> query = db.EditionDistricts;
                editionDistrictList = await query.ToListAsync();

                return editionDistrictList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(EditionDistrict editionDistrict)
        {
            try
            {
                await db.EditionDistricts.AddAsync(editionDistrict);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(EditionDistrict editionDistrict)
        {
            try
            {
                var upateEditionDistrict = await db.EditionDistricts.FirstOrDefaultAsync(d => d.AutoId == editionDistrict.AutoId);

                if (upateEditionDistrict == null)
                    return false;



                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(EditionDistrict editionDistrict)
        {
            try
            {
                var removeEditionDistrict = await db.EditionDistricts.FirstOrDefaultAsync(d => d.AutoId == editionDistrict.AutoId);

                if (removeEditionDistrict == null)
                    return false;

                db.EditionDistricts.Remove(removeEditionDistrict);
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
