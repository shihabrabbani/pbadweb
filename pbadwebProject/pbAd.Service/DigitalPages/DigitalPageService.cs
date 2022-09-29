#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Service.DigitalPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.DigitalPages
{
    public class DigitalPageService: IDigitalPageService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public DigitalPageService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods       

        public async Task<DigitalPage> GetDetailsById(int id)
        {
            var single = new DigitalPage();
            try
            {
                single = await db.DigitalPages.FirstOrDefaultAsync(d => d.DigitalPageId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetDigitalPageListForDropdown()
        {
            IQueryable<DigitalPage> query = db.DigitalPages;

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.DigitalPageName,
                Value = f.DigitalPageId.ToString()
            });
        }

        public async Task<bool> Add(DigitalPage subDigitalPage)
        {
            try
            {
                await db.DigitalPages.AddAsync(subDigitalPage);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(DigitalPage subDigitalPage)
        {
            try
            {
                var upateDigitalPage = await db.DigitalPages.FirstOrDefaultAsync(d => d.DigitalPageId == subDigitalPage.DigitalPageId);

                if (upateDigitalPage == null)
                    return false;

                upateDigitalPage.DigitalPageId = subDigitalPage.DigitalPageId;
                upateDigitalPage.DigitalPageName = subDigitalPage.DigitalPageName;
                upateDigitalPage.ModifiedBy = subDigitalPage.ModifiedBy;
                upateDigitalPage.ModifiedDate = DateTime.Now;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(DigitalPage subDigitalPage)
        {
            try
            {
                var removeDigitalPage = await db.DigitalPages.FirstOrDefaultAsync(d => d.DigitalPageId == subDigitalPage.DigitalPageId);

                if (removeDigitalPage == null)
                    return false;

                db.DigitalPages.Remove(removeDigitalPage);
                await db.SaveChangesAsync();

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
