using AutoMapper;
using pbAd.Data.Models;
using pbAd.Web.ViewModels.Advertisers;
using pbAd.Web.ViewModels.Users;

namespace pbAd.Web.Infrastructure.Automappers
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            //consumer
            CreateMap<Advertiser, AdvertiserViewModel>();
            CreateMap<AdvertiserViewModel,Advertiser>();
            
            //user
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
        }
    }
}
