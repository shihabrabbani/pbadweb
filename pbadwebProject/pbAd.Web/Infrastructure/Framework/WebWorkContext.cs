using pbAd.Data.Models;
using pbAd.Service.CacheManagerServices;

namespace pbAd.Web.Infrastructure.Framework
{
    public class WebWorkContext: IWorkContext
    {
        #region Private Members

        private readonly ICacheManagerService cacheManagerService;
        private User _cacheLoggedInUser;

        #endregion

        #region Ctor
        public WebWorkContext(ICacheManagerService cacheManagerService)
        {
            this.cacheManagerService = cacheManagerService;
        }
        #endregion

        #region Current Logged In User
        public User CurrentLoggedInUser
        {
            get
            {
                return GetCurrentLoggedInUser();
            }
            set
            {
                _cacheLoggedInUser = value;
            }
        }
        #endregion

        #region Get Current Logged In User

        public User GetCurrentLoggedInUser()
        {
            if (_cacheLoggedInUser != null)
                return _cacheLoggedInUser;

            _cacheLoggedInUser = cacheManagerService.GetCurrentLoggedInUser();
            return _cacheLoggedInUser;
        } 

        #endregion
    }
}
