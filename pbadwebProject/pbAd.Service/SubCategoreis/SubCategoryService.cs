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

namespace pbAd.Service.SubCategories
{
    public class SubCategoryService: ISubCategoryService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public SubCategoryService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<SubCategory>> GetListByFilter(SubCategorySearchFilter filter)
        {
            var subCategoryList = new List<SubCategory>();

            IQueryable<SubCategory> query = db.SubCategories.Where(f =>
                                     (filter.SearchTerm == string.Empty || f.SubCategoryName.Contains(filter.SearchTerm.Trim())));

            subCategoryList = await query.ToListAsync();

            return subCategoryList;
        }

        public async Task<SubCategory> GetDetailsById(int id)
        {
            var single = new SubCategory();
            try
            {
                single = await db.SubCategories.FirstOrDefaultAsync(d => d.SubCategoryId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoryByCategory(int categoryId)
        {
            var subCategoryList = new List<SubCategory>();

            try
            {
                IQueryable<SubCategory> query = db.SubCategories.Where(f =>f.CategoryId==categoryId);
                subCategoryList = await query.ToListAsync();

                return subCategoryList;
            }
            catch (Exception ex)
            {
                return new List<SubCategory>();
            }
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetSubCategoryListForDropdown(int categoryId)
        {
            IQueryable<SubCategory> query = db.SubCategories.Where(f=>f.CategoryId== categoryId);

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.SubCategoryName,
                Value = f.SubCategoryId.ToString()
            });
        }

        public async Task<bool> Add(SubCategory subCategory)
        {
            try
            {
                await db.SubCategories.AddAsync(subCategory);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(SubCategory subCategory)
        {
            try
            {
                var upateSubCategory = await db.SubCategories.FirstOrDefaultAsync(d => d.SubCategoryId == subCategory.SubCategoryId);

                if (upateSubCategory == null)
                    return false;

                upateSubCategory.CategoryId = subCategory.CategoryId;
                upateSubCategory.SubCategoryName = subCategory.SubCategoryName;
               // upateSubCategory.ModifiedBy = subCategory.ModifiedBy;
              //  upateSubCategory.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(SubCategory subCategory)
        {
            try
            {
                var removeSubCategory = await db.SubCategories.FirstOrDefaultAsync(d => d.SubCategoryId == subCategory.SubCategoryId);

                if (removeSubCategory == null)
                    return false;

                db.SubCategories.Remove(removeSubCategory);
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
