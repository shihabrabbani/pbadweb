#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Service.DigitalAdUnitTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.DigitalAdUnitTypes
{
    public class DigitalAdUnitTypeService: IDigitalAdUnitTypeService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public DigitalAdUnitTypeService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods       

        public async Task<DigitalAdUnitType> GetDetailsById(int id)
        {
            var single = new DigitalAdUnitType();
            try
            {
                single = await db.DigitalAdUnitTypes.FirstOrDefaultAsync(d => d.DigitalAdUnitTypeId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetDigitalAdUnitTypeListForDropdown()
        {
            IQueryable<DigitalAdUnitType> query = db.DigitalAdUnitTypes;

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.DigitalAdUnitTypeName,
                Value = f.DigitalAdUnitTypeId.ToString()
            });
        }

        public async Task<bool> Add(DigitalAdUnitType subDigitalAdUnitType)
        {
            try
            {
                await db.DigitalAdUnitTypes.AddAsync(subDigitalAdUnitType);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(DigitalAdUnitType subDigitalAdUnitType)
        {
            try
            {
                var upateDigitalAdUnitType = await db.DigitalAdUnitTypes.FirstOrDefaultAsync(d => d.DigitalAdUnitTypeId == subDigitalAdUnitType.DigitalAdUnitTypeId);

                if (upateDigitalAdUnitType == null)
                    return false;

                upateDigitalAdUnitType.DigitalAdUnitTypeId = subDigitalAdUnitType.DigitalAdUnitTypeId;
                upateDigitalAdUnitType.DigitalAdUnitTypeName = subDigitalAdUnitType.DigitalAdUnitTypeName;
                upateDigitalAdUnitType.ModifiedBy = subDigitalAdUnitType.ModifiedBy;
                upateDigitalAdUnitType.ModifiedDate = DateTime.Now;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(DigitalAdUnitType subDigitalAdUnitType)
        {
            try
            {
                var removeDigitalAdUnitType = await db.DigitalAdUnitTypes.FirstOrDefaultAsync(d => d.DigitalAdUnitTypeId == subDigitalAdUnitType.DigitalAdUnitTypeId);

                if (removeDigitalAdUnitType == null)
                    return false;

                db.DigitalAdUnitTypes.Remove(removeDigitalAdUnitType);
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
