#region Usings

using pbAd.Data.Models;
using System.Threading.Tasks;

#endregion

namespace pbAd.Service.CacheManagerServices
{
    public interface ICacheManagerService
    {
        User GetCurrentLoggedInUser();       
        Task<User> GetLoggedInUser();
        void RemoveCurrentLoggedInUserCacheInfo(int consumerId);
        void SetCurrentLoggedInUser(User user);

        User GetSSLInUserInfo(string key);
        void SetSSLKeysValue(string key, int value);
        void RemoveSSLUserCacheInfo(string key);
    }
}
