using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.UserGroups
{
    public interface IUserGroupService
    {
        Task<UserGroup> GetDetailsById(int id);
        Task<IEnumerable<UserGroup>> GetAllUserGroups();
        Task<bool> Add(UserGroup userGroup);
        Task<bool> Update(UserGroup userGroup);
        Task<bool> Remove(UserGroup userGroup);
    }
}
