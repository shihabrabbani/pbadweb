using Microsoft.EntityFrameworkCore;
using System;
using pbAd.Data.Models;
using pbAd.Data.Views;
using pbAd.Data.DomainModels.AdBookings;

namespace pbAd.Service
{
    public class pbAdContext : DbContext
    {
        //create constructor
        public pbAdContext(DbContextOptions<pbAdContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        //Classified Text
        public DbSet<ABPrintClassifiedText> ABPrintClassifiedTexts { get; set; }
        public DbSet<ABPrintClassifiedTextDetail> ABPrintClassifiedTextDetails { get; set; }
        public DbSet<RatePrintClassifiedText> RatePrintClassifiedTexts { get; set; }

        //Classified Display
        public DbSet<ABPrintClassifiedDisplay> ABPrintClassifiedDisplays { get; set; }
        public DbSet<ABPrintClassifiedDisplayDetail> ABPrintClassifiedDisplayDetails { get; set; }
        public DbSet<RatePrintClassifiedDisplay> RatePrintClassifiedDisplays { get; set; }

        //Private Display
        public DbSet<ABPrintPrivateDisplay> ABPrintPrivateDisplays { get; set; }
        public DbSet<ABPrintPrivateDisplayDetail> ABPrintPrivateDisplayDetails { get; set; }
        public DbSet<RatePrintPrivateDisplay> RatePrintPrivateDisplays { get; set; }


        //Digital Display
        public DbSet<ABDigitalDisplay> ABDigitalDisplays { get; set; }
        public DbSet<ABDigitalDisplayDetail> ABDigitalDisplayDetails { get; set; }
        public DbSet<DigitalDisplayMediaContent> DigitalDisplayMediaContents { get; set; }
        public DbSet<RateDigitalDisplay> RateDigitalDisplays { get; set; }
        public DbSet<DigitalAdUnitType> DigitalAdUnitTypes { get; set; }
        public DbSet<DigitalPage> DigitalPages { get; set; }
        public DbSet<DigitalPagePosition> DigitalPagePositions { get; set; }

        //Rates
        public DbSet<RatePrintGovtAd> RatePrintGovtAds { get; set; }
        public DbSet<RatePrintSpotAd> RatePrintSpotAds { get; set; }
        public DbSet<RatePrintEarPanelAd> RatePrintEarPanelAds { get; set; }
        public DbSet<OfferEdition> OfferEditions { get; set; }

        public DbSet<District> Districts { get; set; }
        public DbSet<Upazilla> Upazillas { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<EditionDate> EditionDates { get; set; }
        public DbSet<EditionPage> EditionPages { get; set; }
        public DbSet<OfferDate> OfferDates { get; set; }
        public DbSet<DefaultDiscount> DefaultDiscounts { get; set; }

        public DbSet<ACL> ACLs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<BrandRelation> BrandRelations { get; set; }
        public DbSet<PrivateDisplayMediaContent> PrivateDisplayMediaContents { get; set; }
        public DbSet<EditionDistrict> EditionDistricts { get; set; }
        public DbSet<SSLPaymentTransaction> SSLPaymentTransactions { get; set; }

        #region Views
        public virtual DbSet<View_RatePrintPrivateDisplay> View_RatePrintPrivateDisplays { get; set; }
        public virtual DbSet<View_RatePrintSpotAd> View_RatePrintSpotAds { get; set; }
        public virtual DbSet<View_RatePrintEarPanelAd> View_RatePrintEarPanelAds { get; set; }
        public virtual DbSet<View_RatePrintGovtAd> View_RatePrintGovtAds { get; set; }
        public virtual DbSet<View_ABPrintClassifiedText> View_ABPrintClassifiedTexts { get; set; }
        #endregion

        //for stored procedure
        public virtual DbQuery<BookingOrdersModel> BookingOrders { get; set; }

        public virtual DbQuery<UploadLaterOrdersModel> UploadLaterOrders { get; set; }
    }

}
