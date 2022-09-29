using pbAd.Core.Filters;
using pbAd.Data.DomainModels.Accounts;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetListByFilter(UserSearchFilter filter);
        Task<User> GetDetailsById(int id);
        User GetById(int id);
        Task<User> FindByUsername(string username);
        Task<User> AuthenticateUser(string username, string password);
        Task<User> AuthenticateUserB(string username, string password);
        Task<User> FindByUsernameAndEmail(string username, string email, string otp = "");
        Task<User> Add(User user);
        Task<bool> Update(User user);
        Task<bool> Remove(User user);
        Task<PasswordChangeRequest> UpdateUserPassword(PasswordChangeRequest request);
        Task<OTPChangeRequest> UpdateOTP(OTPChangeRequest request);
    }
}
