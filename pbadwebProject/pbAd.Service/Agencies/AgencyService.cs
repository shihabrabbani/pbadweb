#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Service.Agencies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.Agencies
{
    public class AgencyService: IAgencyService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public AgencyService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Agency>> GetAgencyForAutoComplete(AgencySearchFilter filter)
        {
            IQueryable<Agency> query = db.Agencies;

            var itemList = await query
                .Where(f=> filter.SearchTerm==string.Empty || f.AgencyName.StartsWith(filter.SearchTerm))
                .Take(10).OrderBy(f=>f.AgencyName).ToListAsync();

            return itemList;
        }
        public async Task<Agency> GetDetailsById(int id)
        {
            var single = new Agency();
            try
            {
                single = await db.Agencies.FirstOrDefaultAsync(d => d.AgencyId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<Agency> GetDetailsByIdAndName(int id , string agentName)
        {
            var single = new Agency();
            try
            {
                single = await db.Agencies.FirstOrDefaultAsync(d => d.AgencyId == id && d.AgencyName.ToLower().Trim() == agentName.ToLower().Trim());
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetAgencyListForDropdown()
        {
            IQueryable<Agency> query = db.Agencies;

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.AgencyName,
                Value = f.AgencyId.ToString()
            });
        }

        public async Task<bool> Add(Agency agency)
        {
            try
            {
                db.Agencies.Add(agency);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(Agency agency)
        {
            try
            {
                var upateAgency = await db.Agencies.FirstOrDefaultAsync(d => d.AgencyId == agency.AgencyId);

                if (upateAgency == null)
                    return false;

                upateAgency.AgencyId = agency.AgencyId;
                upateAgency.AgencyName = agency.AgencyName;
                upateAgency.ModifiedBy = agency.ModifiedBy;
                upateAgency.ModifiedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(Agency agency)
        {
            try
            {
                var removeAgency = await db.Agencies.FirstOrDefaultAsync(d => d.AgencyId == agency.AgencyId);

                if (removeAgency == null)
                    return false;

                db.Agencies.Remove(removeAgency);
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
