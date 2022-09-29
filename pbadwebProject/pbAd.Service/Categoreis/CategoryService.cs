#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Service.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.Categories
{
    public class CategoryService: ICategoryService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public CategoryService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Category>> GetListByFilter(CategorySearchFilter filter)
        {
            var subCategoryList = new List<Category>();

            IQueryable<Category> query = db.Categories.Where(f =>
                                     (filter.SearchTerm == string.Empty || f.CategoryName.Contains(filter.SearchTerm.Trim())));

            subCategoryList = await query.ToListAsync();

            return subCategoryList;
        }

        public async Task<Category> GetDetailsById(int id)
        {
            var single = new Category();
            try
            {
                single = await db.Categories.FirstOrDefaultAsync(d => d.CategoryId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetCategoryListForDropdown(CategorySearchFilter filter)
        {
            IQueryable<Category> query = db.Categories.Where(f=>f.CategoryType== filter.CategoryType);

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.CategoryName,
                Value = f.CategoryId.ToString()
            });
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetCategoryListForDropdownByBrandId(int brandId)
        {
            IQueryable<BrandRelation> query = db.BrandRelations.Where(f => f.BrandId == brandId);

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.CategoryName,
                Value = f.CategoryId.ToString()
            });
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetCategoryListForDropdownDisplay()
        {
            IQueryable<Category> query = db.Categories.Where(f => f.CategoryType == 1);

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.CategoryName,
                Value = f.CategoryId.ToString()
            });
        }
        public async Task<bool> Add(Category subCategory)
        {
            try
            {
                db.Categories.Add(subCategory);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(Category subCategory)
        {
            try
            {
                var upateCategory = await db.Categories.FirstOrDefaultAsync(d => d.CategoryId == subCategory.CategoryId);

                if (upateCategory == null)
                    return false;

                upateCategory.CategoryId = subCategory.CategoryId;
                upateCategory.CategoryName = subCategory.CategoryName;
                upateCategory.ModifiedBy = subCategory.ModifiedBy;
                upateCategory.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(Category subCategory)
        {
            try
            {
                var removeCategory = await db.Categories.FirstOrDefaultAsync(d => d.CategoryId == subCategory.CategoryId);

                if (removeCategory == null)
                    return false;

                db.Categories.Remove(removeCategory);
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
