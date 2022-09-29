#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Service.DigitalPagePositions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.DigitalPagePositions
{
    public class DigitalPagePositionService: IDigitalPagePositionService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public DigitalPagePositionService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods       

        public async Task<DigitalPagePosition> GetDetailsById(int id)
        {
            var single = new DigitalPagePosition();
            try
            {
                single = await db.DigitalPagePositions.FirstOrDefaultAsync(d => d.DigitalPagePositionId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetDigitalPagePositionListForDropdown(int? pageId)
        {
            IQueryable<DigitalPagePosition> query = db.DigitalPagePositions;

            var itemList = await query.Where(f=>
            (pageId ==null || pageId ==0 || f.DigitalPageId==pageId)
            ).ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.DigitalPagePositionName,
                Value = f.DigitalPagePositionId.ToString()
            });
        }

        public async Task<bool> Add(DigitalPagePosition subDigitalPagePosition)
        {
            try
            {
                await db.DigitalPagePositions.AddAsync(subDigitalPagePosition);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(DigitalPagePosition subDigitalPagePosition)
        {
            try
            {
                var upateDigitalPagePosition = await db.DigitalPagePositions.FirstOrDefaultAsync(d => d.DigitalPagePositionId == subDigitalPagePosition.DigitalPagePositionId);

                if (upateDigitalPagePosition == null)
                    return false;

                upateDigitalPagePosition.DigitalPagePositionId = subDigitalPagePosition.DigitalPagePositionId;
                upateDigitalPagePosition.DigitalPagePositionName = subDigitalPagePosition.DigitalPagePositionName;
                upateDigitalPagePosition.ModifiedBy = subDigitalPagePosition.ModifiedBy;
                upateDigitalPagePosition.ModifiedDate = DateTime.Now;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(DigitalPagePosition subDigitalPagePosition)
        {
            try
            {
                var removeDigitalPagePosition = await db.DigitalPagePositions.FirstOrDefaultAsync(d => d.DigitalPagePositionId == subDigitalPagePosition.DigitalPagePositionId);

                if (removeDigitalPagePosition == null)
                    return false;

                db.DigitalPagePositions.Remove(removeDigitalPagePosition);
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
