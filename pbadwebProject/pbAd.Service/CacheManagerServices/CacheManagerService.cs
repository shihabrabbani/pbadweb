#region Usings

using pbAd.Core.Utilities;
using pbAd.Data.Models;
using pbAd.Service.Users;
using pbAd.Service.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace pbAd.Service.CacheManagerServices
{
    public class CacheManagerService : ICacheManagerService
    {
        #region Private Members

        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly int _cacheExpirationHours=2;

        #endregion

        #region Ctor
        public CacheManagerService(IMemoryCache memoryCache,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor,
           IConfiguration configuration)
        {
            this.memoryCache = memoryCache;
            this.configuration = configuration;
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Get Current Logged InUser
        public User GetCurrentLoggedInUser()
        {
            //Get user Id
            var userId = GetUserId();
            if (userId == null) return null;

            var currentLoggedInUser = new User();

            var cacheKey = $"{CacheKeyConstants.UserProfile}-{userId}";

            try
            { 
                if (!memoryCache.TryGetValue(cacheKey, out currentLoggedInUser))
                {
                    if (currentLoggedInUser == null || currentLoggedInUser.UserId<=0)
                    {
                        currentLoggedInUser = userService.GetById((int)userId);
                    }

                    //let's memory cache entry options
                    var cacheEntryOptions = new MemoryCacheEntryOptions();
                    cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromHours(_cacheExpirationHours));

                    // here we need to set the value for cache memory
                    memoryCache.Set(cacheKey, currentLoggedInUser, cacheEntryOptions);                    
                }
            }
            catch (Exception ex)
            {
                //remove cache key
                RemoveCacheMemory(cacheKey);

                return null;
            }

            return currentLoggedInUser;
        }

        #endregion

        #region Get Current Logged InUser
        public async Task<User> GetLoggedInUser()
        {
            //Get User Id
            var userId = GetUserId();
            if (userId == null) return null;

            var currentLoggedInUser = new User();

            var cacheKey = $"{CacheKeyConstants.UserProfile}-{userId}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out currentLoggedInUser))
                {
                    if (currentLoggedInUser == null || currentLoggedInUser.UserId <= 0)
                    {
                        currentLoggedInUser = await userService.GetDetailsById((int)userId);
                    }

                    //let's memory cache entry options
                    var cacheEntryOptions = new MemoryCacheEntryOptions();
                    cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromHours(_cacheExpirationHours));

                    // here we need to set the value for cache memory
                    memoryCache.Set(cacheKey, currentLoggedInUser, cacheEntryOptions);
                }
            }
            catch (Exception ex)
            {
                //remove cache key
                RemoveCacheMemory(cacheKey);

                return null;
            }

            return currentLoggedInUser;
        }

        #endregion

        #region Set Current Logged In User
        public void SetCurrentLoggedInUser(User user)
        {
            var cacheKey = $"{CacheKeyConstants.UserProfile}-{user.UserId}";

            try
            {
                //remove cache key
                RemoveCacheMemory(cacheKey);

                //let's memory cache entry options
                var cacheEntryOptions = new MemoryCacheEntryOptions();
                cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromHours(_cacheExpirationHours));

                // here we need to set the value for cache memory
                memoryCache.Set(cacheKey, user, cacheEntryOptions);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Remove Current Logged In User Cache Info

        public void RemoveCurrentLoggedInUserCacheInfo(int userId)
        {            
            try
            {
                var cacheKey = $"{CacheKeyConstants.UserProfile}-{userId}";
                //lets remove user info from cache memory
                RemoveCacheMemory(cacheKey);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region SSL User Cache Info

        public void RemoveSSLUserCacheInfo(string key)
        {
            try
            {
                var cacheKey = $"{CacheKeyConstants.SSLCacheKey}-{key}";
                //lets remove user info from cache memory
                RemoveCacheMemory(cacheKey);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region SSL User Key 

        public User GetSSLInUserInfo(string key)
        {            
            var currentLoggedInUser = new User();

            //get ssl key
            int userId = GetSSLUserId(key);
            if (userId <= 0) 
                return currentLoggedInUser;
                        
            var cacheKey = $"{CacheKeyConstants.UserProfile}-{userId}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out currentLoggedInUser))
                {
                    if (currentLoggedInUser == null || currentLoggedInUser.UserId <= 0)
                    {
                        currentLoggedInUser = userService.GetById((int)userId);
                    }

                    //let's memory cache entry options
                    var cacheEntryOptions = new MemoryCacheEntryOptions();
                    cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromHours(_cacheExpirationHours));

                    // here we need to set the value for cache memory
                    memoryCache.Set(cacheKey, currentLoggedInUser, cacheEntryOptions);
                }
            }
            catch (Exception ex)
            {
                //remove cache key
                RemoveCacheMemory(cacheKey);

                return null;
            }

            return currentLoggedInUser;
        }


        public void SetSSLKeysValue(string key, int value)
        {
            var cacheKey = $"{CacheKeyConstants.SSLCacheKey}-{key}";

            try
            {
                //remove cache key
                RemoveCacheMemory(cacheKey);

                //let's memory cache entry options
                var cacheEntryOptions = new MemoryCacheEntryOptions();
                cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(5));

                // here we need to set the value for cache memory
                memoryCache.Set(cacheKey, value, cacheEntryOptions);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Private Methods   

        private int GetSSLUserId(string key)
        {
            int userId = 0;

            var cacheKey = $"{CacheKeyConstants.SSLCacheKey}-{key}";

            try
            {
                if (!memoryCache.TryGetValue(cacheKey, out userId))
                {
                    userId = 0;
                }
            }
            catch (Exception ex)
            {
                //remove cache key
                RemoveCacheMemory(cacheKey);

                userId = 0;
            }

            return userId;
        }

        private int? GetUserId()
        {
            int? userId = null;

            //get claims from user identity
            var userClaims = httpContextAccessor.HttpContext.User.Claims;
            if (userClaims == null) return null;

            //get user id
            var userIdClaim = userClaims.FirstOrDefault(c => c.Type == "UserId").Value;
            if (userIdClaim == null || !userIdClaim.Any()) return null;

            userId = Convert.ToInt32(userIdClaim);

            return userId;
        }

        private void RemoveCacheMemory(string cacheKey)
        {
            memoryCache.Remove(cacheKey);
        }

        #endregion
    }
}
