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

namespace pbAd.Service.EditionPages
{
    public class EditionPageService: IEditionPageService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public EditionPageService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<EditionPage>> GetListByFilter(EditionPageSearchFilter filter)
        {
            var editionList = new List<EditionPage>();

            IQueryable<EditionPage> query = db.EditionPages;

            editionList = await query.ToListAsync();

            return editionList;
        }

        public async Task<EditionPage> GetDetailsById(int id)
        {
            var single = new EditionPage();
            try
            {
                single = await db.EditionPages.AsNoTracking().FirstOrDefaultAsync(d => d.EditionPageId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<EditionPage> GetEdtionPageByEditionAndPageNo(int editionId, int pageNumber)
        {
            var single = new EditionPage();
            try
            {
                single = await db.EditionPages.FirstOrDefaultAsync(d => d.EditionId == editionId && d.EditionPageNo==pageNumber);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<EditionPage>> GetAllEditionPages()
        {
            var editionList = new List<EditionPage>();

            try
            {
                IQueryable<EditionPage> query = db.EditionPages;
                editionList = await query.ToListAsync();

                return editionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<EditionPage>> GetEditionPagesByEditionAndPageNo(List<int> editionIds, int pageNo)
        {
            var editionList = new List<EditionPage>();
            if (editionIds == null)
                editionIds = new List<int>();

            try
            {
                IQueryable<EditionPage> query = db.EditionPages;
                editionList = await query.Where(f=>(editionIds.Any(a=>a==f.EditionId)) && f.EditionPageNo==pageNo).ToListAsync();

                return editionList;
            }
            catch (Exception ex)
            {
                return null;
            }           
        }
        public async Task<IEnumerable<ConstantDropdownItem>> GetEditionPageListForDropdown(int editionId)
        {
            IQueryable<EditionPage> query = db.EditionPages.Where(f=>f.EditionId==editionId);

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = $"{f.EditionPageName} (Page - {f.EditionPageNo})",
                Value = f.EditionPageId.ToString(),
            });
        }
        public async Task<IEnumerable<EditionPage>> GetAllEditionPagesByIds(List<int> editionIds)
        {
            var editionList = new List<EditionPage>();

            try
            {
                IQueryable<EditionPage> query = db.EditionPages;
                editionList = await query.Where(f=> editionIds.Any(a=>a==f.EditionPageId)).ToListAsync();

                return editionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Add(EditionPage edition)
        {
            try
            {
                await db.EditionPages.AddAsync(edition);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(EditionPage edition)
        {
            try
            {
                var upateEditionPage = await db.EditionPages.FirstOrDefaultAsync(d => d.EditionPageId == edition.EditionPageId);

                if (upateEditionPage == null)
                    return false;
                
                upateEditionPage.ModifiedBy = edition.ModifiedBy;
                upateEditionPage.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(EditionPage edition)
        {
            try
            {
                var removeEditionPage = await db.EditionPages.FirstOrDefaultAsync(d => d.EditionPageId == edition.EditionPageId);

                if (removeEditionPage == null)
                    return false;

                db.EditionPages.Remove(removeEditionPage);
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
