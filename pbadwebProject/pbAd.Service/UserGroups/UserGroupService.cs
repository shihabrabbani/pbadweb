#region Usings
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.UserGroups
{
    public class UserGroupService: IUserGroupService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public UserGroupService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
       
        public async Task<UserGroup> GetDetailsById(int id)
        {
            var single = new UserGroup();
            try
            {
                single = await db.UserGroups.FirstOrDefaultAsync(d => d.GroupId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<UserGroup>> GetAllUserGroups()
        {
            var brandList = new List<UserGroup>();

            try
            {
                IQueryable<UserGroup> query = db.UserGroups;
                brandList = await query.ToListAsync();

                return brandList;
            }
            catch (Exception ex)
            {
                return null;
            }           
        }

        public async Task<bool> Add(UserGroup brand)
        {
            try
            {
                await db.UserGroups.AddAsync(brand);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(UserGroup brand)
        {
            try
            {
                var upateUserGroup = await db.UserGroups.FirstOrDefaultAsync(d => d.GroupId == brand.GroupId);

                if (upateUserGroup == null)
                    return false;
                
                upateUserGroup.GroupName = brand.GroupName;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(UserGroup brand)
        {
            try
            {
                var removeUserGroup = await db.UserGroups.FirstOrDefaultAsync(d => d.GroupId == brand.GroupId);

                if (removeUserGroup == null)
                    return false;

                db.UserGroups.Remove(removeUserGroup);
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
