
#region Usings

using AutoMapper;
using pbAd.Service;
using pbAd.Service.ABDigitalDisplays;
using pbAd.Service.ABPrintClassifiedDisplays;
using pbAd.Service.ABPrintClassifiedTexts;
using pbAd.Service.ABPrintPrivateDisplays;
using pbAd.Service.Agencies;
using pbAd.Service.Brands;
using pbAd.Service.CacheManagerServices;
using pbAd.Service.Categories;
using pbAd.Service.Advertisers;
using pbAd.Service.DigitalAdUnitTypes;
using pbAd.Service.DigitalPagePositions;
using pbAd.Service.DigitalPages;
using pbAd.Service.Editions;
using pbAd.Service.OfferDates;
using pbAd.Service.RateDigitalDisplays;
using pbAd.Service.RatePrintClassifiedDisplays;
using pbAd.Service.RatePrintClassifiedTexts;
using pbAd.Service.RatePrintGovtAds;
using pbAd.Service.RatePrintPrivateDisplays;
using pbAd.Service.RatePrintSpotAds;
using pbAd.Service.SubCategories;
using pbAd.Service.UserGroups;
using pbAd.Service.Users;
using pbAd.Web.Infrastructure.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using pbAd.Service.OfferEditions;
using pbAd.Service.DefaultDiscounts;
using pbAd.Service.EditionPages;
using pbAd.Service.ABPrintPrivateDisplayGovts;
using pbAd.Service.EditionDistricts;
using pbAd.Service.EmailSenders;
using pbAd.Service.RatePrintEarPanelAds;

using pbAd.Service.AdBookingReports;
using pbAd.Service.PaymentGateways;
using pbAd.Service.SSLPaymentTransactions;

#endregion

namespace pbAd.Web
{
    public class Startup
    {
        #region Public Methods
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<pbAdContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("pbAdContext"));
            });

            //configure sociallogins
            //ConfigureSocialLogins(services);

            //for authentication
            FormAuthenticationImplementation(services);

            //DI Registrations
            DependencyInjectionResolver(services);

            //enable automapper
            services.AddAutoMapper(typeof(Startup));

            //add session time
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {                
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        #endregion

        #region Private Methods

        //private void ConfigureSocialLogins(IServiceCollection services)
        //{
        //    //social login
        //    services.AddAuthentication()
        //        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        //        .AddGoogle(options =>
        //        {
        //            options.ClientId = Configuration["pbAdSocialLogin:Google:ClientId"];
        //            options.ClientSecret = Configuration["pbAdSocialLogin:Google:ClientSecret"];
        //        })
        //        .AddFacebook(options =>
        //        {
        //            options.ClientId = Configuration["pbAdSocialLogin:Facebook:ClientId"];
        //            options.ClientSecret = Configuration["pbAdSocialLogin:Facebook:ClientSecret"];
        //            //options.CallbackPath = "/Account/ExternalLoginCallback";
        //        });
        //}

        private void FormAuthenticationImplementation(IServiceCollection services)
        {
            services.AddAuthentication(Configuration["pbAdFormAuthentication:FormAuthenticationKey"])
                            .AddCookie(Configuration["pbAdFormAuthentication:FormAuthenticationKey"],
                            options =>
                            {
                                options.Cookie.Name = Configuration["pbAdFormAuthentication:FormAuthenticationWithCookieName"];
                                options.LoginPath = Configuration["pbAdFormAuthentication:LoginPath"];
                                options.AccessDeniedPath = Configuration["pbAdFormAuthentication:AccessDeniedPath"];
                            });
        }

        private void DependencyInjectionResolver(IServiceCollection services)
        {
            //DI Registrations            
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdvertiserService, AdvertiserService>();
            services.AddScoped<ICacheManagerService, CacheManagerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IABPrintClassifiedTextService, ABPrintClassifiedTextService>();
            services.AddScoped<IEditionService, EditionService>();
            services.AddScoped<IRatePrintClassifiedTextService, RatePrintClassifiedTextService>();
            services.AddScoped<IABPrintClassifiedDisplayService, ABPrintClassifiedDisplayService>();
            services.AddScoped<IRatePrintClassifiedDisplayService, RatePrintClassifiedDisplayService>();
            services.AddScoped<IRatePrintGovtAdService, RatePrintGovtAdService>();
            services.AddScoped<IRatePrintPrivateDisplayService, RatePrintPrivateDisplayService>();
            services.AddScoped<IRatePrintSpotAdService, RatePrintSpotAdService>();
            services.AddScoped<IRatePrintEarPanelAdService, RatePrintEarPanelAdService>();
            services.AddScoped<Service.ABPrintPrivateDisplays.IABPrintPrivateDisplayService, ABPrintPrivateDisplayService>();
            services.AddScoped<IABDigitalDisplayService, ABDigitalDisplayService>();
            services.AddScoped<IRateDigitalDisplayService, RateDigitalDisplayService>();

            services.AddScoped<IDigitalAdUnitTypeService, DigitalAdUnitTypeService>();
            services.AddScoped<IDigitalPageService, DigitalPageService>();
            services.AddScoped<IDigitalPagePositionService, DigitalPagePositionService>();

            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<IOfferDateService, OfferDateService>();
            services.AddScoped<IOfferEditionService, OfferEditionService>();
            services.AddScoped<IDefaultDiscountService, DefaultDiscountService>();
            services.AddScoped<IEditionPageService, EditionPageService>();
            services.AddScoped<Service.ABPrintPrivateDisplayGovts.IABPrintPrivateDisplayGovtService, ABPrintPrivateDisplayGovtService>();
            services.AddScoped<IEditionDistrictService, EditionDistrictService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
          
            services.AddScoped<IAdBookingReportService, AdBookingReportService>();

            services.AddScoped<ISSLPaymentTransactionService, SSLPaymentTransactionService>();
            services.AddScoped<IWorkContext, WebWorkContext>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        #endregion
    }
}
