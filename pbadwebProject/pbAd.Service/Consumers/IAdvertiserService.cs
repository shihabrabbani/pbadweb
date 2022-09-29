using pbAd.Core.Filters;
using pbAd.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pbAd.Service.Advertisers
{
    public interface IAdvertiserService
    {
        Task<IEnumerable<Advertiser>> GetListByFilter(AdvertiserSearchFilter filter);
        Task<Advertiser> GetDetailsById(int id);
        Advertiser GetByAdvertiserId(int id);
        Task<Advertiser> GetDetailsByMobile(string mobileNo);
        Task<Advertiser> GetAdvirtizerInfoByFilter(AdvertiserSearchFilter filter);
        Task<IEnumerable<Advertiser>> GetAdvertiserForAutoComplete(AdvertiserSearchFilter filter);
        Task<Advertiser> GetAdvirtizerInfo(int advertiserId, string mobileNo, bool personal);
        Task<bool> Add(Advertiser consumer);
        Task<bool> Update(Advertiser consumer);
        Task<bool> Remove(Advertiser consumer);
    }
}
