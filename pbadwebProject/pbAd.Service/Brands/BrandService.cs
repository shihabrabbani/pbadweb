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

namespace pbAd.Service.Brands
{
    public class BrandService : IBrandService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public BrandService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods

        public async Task<Brand> GetDetailsById(int id)
        {
            var single = new Brand();
            try
            {
                single = await db.Brands.FirstOrDefaultAsync(d => d.BrandId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetBrandListForDropdown()
        {
            IQueryable<Brand> query = db.Brands;

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.BrandName,
                Value = f.BrandId.ToString()
            });
        }

        public async Task<IEnumerable<Brand>> GetBrandForAutoComplete(BrandSearchFilter filter)
        {
            IQueryable<Brand> query = db.Brands;

            var itemList = await query
                .Where(f => filter.SearchTerm == string.Empty || f.BrandName.Contains(filter.SearchTerm))
                .OrderBy(o=>o.BrandName)
                .Take(10).ToListAsync();

            return itemList;
        }
        public async Task<BrandRelation> GetBrandWiseAdvirtiser(BrandSearchFilter filter)
        {
            var single = new BrandRelation();
            try
            {
                single = await db.BrandRelations.FirstOrDefaultAsync(d => d.BrandId == filter.BrandId);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            var brandList = new List<Brand>();

            try
            {
                IQueryable<Brand> query = db.Brands;
                brandList = await query.ToListAsync();

                return brandList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(Brand brand)
        {
            try
            {
                await db.Brands.AddAsync(brand);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(Brand brand)
        {
            try
            {
                var upateBrand = await db.Brands.FirstOrDefaultAsync(d => d.BrandId == brand.BrandId);

                if (upateBrand == null)
                    return false;

                upateBrand.ModifiedBy = brand.ModifiedBy;
                upateBrand.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(Brand brand)
        {
            try
            {
                var removeBrand = await db.Brands.FirstOrDefaultAsync(d => d.BrandId == brand.BrandId);

                if (removeBrand == null)
                    return false;

                db.Brands.Remove(removeBrand);
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
