#region Usings
using pbAd.Core.Filters;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.Advertisers
{
    public class AdvertiserService : IAdvertiserService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public AdvertiserService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Advertiser>> GetListByFilter(AdvertiserSearchFilter filter)
        {
            var userList = new List<Advertiser>();

            IQueryable<Advertiser> query = db.Advertisers.Where(f =>
                                     (filter.SearchTerm == string.Empty || f.AdvertiserName.Contains(filter.SearchTerm.Trim())));

            userList = await query.ToListAsync();

            return userList;
        }

        public async Task<Advertiser> GetDetailsById(int id)
        {
            var single = new Advertiser();
            try
            {
                single = await db.Advertisers.FirstOrDefaultAsync(d => d.AdvertiserId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<Advertiser> GetAdvirtizerInfo(int advertiserId, string mobileNo,bool personal)
        {
            var single = new Advertiser();
            try
            {
                if (advertiserId>0)
                {
                    single = await db.Advertisers.FirstOrDefaultAsync(d =>d.AdvertiserId==advertiserId);
                    if(single.AdvertiserName==mobileNo)
                    return single;
                    else
                    {
                        return null;
                    }
                }
                      
            }
            catch (Exception ex)
            {
                return null;
            }

            return single;
        }

        public async Task<Advertiser> GetAdvirtizerInfoByFilter(AdvertiserSearchFilter filter)
        {
            var single = new Advertiser();
            try
            {
                if (filter.AdvertiserId > 0)
                {
                    single = await db.Advertisers.FirstOrDefaultAsync(d => d.AdvertiserId == filter.AdvertiserId);
                    if (single.AdvertiserName == filter.MobileNo)
                        return single;
                    else
                        return null;
                }

                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return single;
        }

        public async Task<IEnumerable<Advertiser>> GetAdvertiserForAutoComplete(AdvertiserSearchFilter filter)
        {
            IQueryable<Advertiser> query = db.Advertisers;

            var itemList = await query
                .Where(f =>
                (filter.AdvertiserType ==null || filter.AdvertiserType==0 || f.AdvertiserType== filter.AdvertiserType) && 
                (filter.SearchTerm == string.Empty || f.AdvertiserName.StartsWith(filter.SearchTerm))
                ).Take(10).OrderBy(f => f.AdvertiserName).ToListAsync();

            return itemList;

        }
        public async Task<Advertiser> GetDetailsByMobile(string mobileNo)
        {
            var single = new Advertiser();
            try
            {
                single = await db.Advertisers.FirstOrDefaultAsync(d => d.AdvertiserMobileNo.Trim() == mobileNo.Trim());
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public Advertiser GetByAdvertiserId(int id)
        {
            var single = new Advertiser();
            try
            {
                single = db.Advertisers.FirstOrDefault(d => d.AdvertiserId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<bool> Add(Advertiser user)
        {
            try
            {
                db.Advertisers.Add(user);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(Advertiser user)
        {
            var currentDate = DateTime.Now;
            try
            {

                var upateAdvertiser = await db.Advertisers.FirstOrDefaultAsync(d => d.AdvertiserId == user.AdvertiserId);

                if (upateAdvertiser == null)
                    return false;

                //upateAdvertiser.AdvertiserId = user.AdvertiserId;
                //upateAdvertiser.AdvertiserName = user.AdvertiserName;
                //upateAdvertiser.FullName = user.FullName;
                //upateAdvertiser.MobileNo = user.MobileNo;
                //upateAdvertiser.PasswordHash = user.PasswordHash;
                //upateAdvertiser.PasswordSalt = user.PasswordSalt;
                //upateAdvertiser.PasswordResetToken = user.PasswordResetToken;
                //upateAdvertiser.IsActive = user.IsActive;
                //upateAdvertiser.Email = user.Email;
                //upateAdvertiser.Designation = user.Designation;
                //upateAdvertiser.EditionId = user.EditionId;
                //upateAdvertiser.DistrictId = user.DistrictId;
                //upateAdvertiser.UpazillaId = user.UpazillaId;
                upateAdvertiser.ModifiedBy = user.ModifiedBy;
                upateAdvertiser.ModifiedDate = currentDate;


                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(Advertiser user)
        {
            try
            {
                var removeAdvertiser = await db.Advertisers.FirstOrDefaultAsync(d => d.AdvertiserId == user.AdvertiserId);

                if (removeAdvertiser == null)
                    return false;

                //removeAdvertiser.IsActive = false;
                removeAdvertiser.ModifiedBy = user.ModifiedBy;
                removeAdvertiser.ModifiedDate = DateTime.Now;

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
