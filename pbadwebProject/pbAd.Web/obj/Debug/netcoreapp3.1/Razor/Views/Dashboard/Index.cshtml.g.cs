#pragma checksum "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Dashboard\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7bf117c230defd4aa743f88bf05540b7f42541e7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_Index), @"mvc.1.0.view", @"/Views/Dashboard/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.Account;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.RndContents;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.AddBooks;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Core.Utilities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.Infrastructure.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Core.Filters;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.AdBookingReports;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using X.PagedList;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using X.PagedList.Mvc.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.Infrastructure.Framework;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Service.CacheManagerServices;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Data.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7bf117c230defd4aa743f88bf05540b7f42541e7", @"/Views/Dashboard/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5ef2d37f717a2bca4e15fa34d68c1dd740ac897c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Dashboard_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<pbAd.Web.ViewModels.Home.DashboardHomeViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Dashboard\Index.cshtml"
  
    ViewData["Title"] = "Dashboard";
    Layout = "~/Views/Shared/_Layout.cshtml";
    var version = Configuration.GetSection("Proshar")["Version"];

#line default
#line hidden
#nullable disable
            DefineSection("BookingWidget", async() => {
                WriteLiteral(@"
    <div class=""steps"">
        <ul>
            <li class=""active"">
                <span>1</span>
                Start
            </li>
            <li>
                <span>2</span>
                COMPOSE AD
            </li>
            <li>
                <span>3</span>
                PAYMENT
            </li>
        </ul>
    </div>
");
            }
            );
            WriteLiteral(@"<div class=""row"">
    <div class=""col-md-6"">
        <div class=""row"">
            <div class=""col-sm-5"">
                <div class=""advertise-img"">
                    <img src=""img/Classified-Text-6.jpg"">
                </div>
            </div>
            <div class=""col-sm-7"">
                <div class=""advertise-content"">
                    <h4>ক্ল্যাসিফাইড টেক্সট বিজ্ঞাপন</h4>
                    <p>শব্দ-ভিত্তিক বিজ্ঞাপন দিতে এখানে <span><a href=""/adbook/booknow"">ক্লিক</a></span> করুন</p>
");
            WriteLiteral(@"                </div>
            </div>
        </div>
    </div>

    <!--<div class=""col-md-6"">
        <div class=""row"">
            <div class=""col-sm-5"">
                <div class=""advertise-img"">
                    <img src=""img/Classified Display-3.jpg"">
                </div>
            </div>
            <div class=""col-sm-7"">
                <div class=""advertise-content"">
                    <h4>ক্ল্যাসিফাইড ডিসপ্লে বিজ্ঞাপন</h4>
                    <p>বিয়েশাদি, জন্মদিন, বিয়েবার্ষিকী, শোক সংবাদ ইত্যাদির বিজ্ঞাপন দিতে এখানে <span><a href=""/bookclasssifieddisplayad/booknow"">ক্লিক</a></span> করুন </p>-->
");
            WriteLiteral(@"                <!--</div>
            </div>
        </div>
    </div>-->

    <div class=""col-md-6"">
        <div class=""row"">
            <div class=""col-sm-5"">
                <div class=""advertise-img"">
                    <img src=""img/Privet.jpg"">
                </div>
            </div>
            <div class=""col-sm-7"">
                <div class=""advertise-content"">
                    <h4>প্রাইভেট ডিসপ্লে বিজ্ঞাপন</h4>
                    <p>বিভিন্ন প্রতিষ্ঠানের বা পণ্যের বিজ্ঞাপন দিতে এখানে <span><a href=""/bookprivatedisplayad/booknow"">ক্লিক</a></span> করুন</p>
");
            WriteLiteral(@"                </div>
            </div>
        </div>
    </div>

    <div class=""col-md-6"">
        <div class=""row"">
            <div class=""col-sm-5"">
                <div class=""advertise-img"">
                    <img src=""img/Government-1.jpg"">
                </div>
            </div>
            <div class=""col-sm-7"">
                <div class=""advertise-content"">
                    <h4>সরকারি বিজ্ঞাপন</h4>
                    <p>সরকারি প্রতিষ্ঠানের দরপত্র, নিয়োগ ইত্যাদির বিজ্ঞাপন দিতে এখানে <span><a href=""/bookgovtprivatedisplayad/booknow"">ক্লিক</a></span> করুন</p>
");
            WriteLiteral(@"                </div>
            </div>
        </div>
    </div>


    <div class=""col-md-6"">
        <div class=""row"">
            <div class=""col-sm-5"">
                <div class=""advertise-img"">
                    <img src=""img/Digital-Ad.jpg"">
                </div>
            </div>
            <div class=""col-sm-7"">
                <div class=""advertise-content"">
                    <h4>ডিজিটাল বিজ্ঞাপন</h4>
                    <p>অনলাইনে বিজ্ঞাপন দিতে এখানে <span><a href=""/bookdigitaldisplayad/booknow"">ক্লিক</a></span> করুন</p>
");
            WriteLiteral("                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n\r\n");
            }
            );
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IConfiguration Configuration { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<pbAd.Web.ViewModels.Home.DashboardHomeViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
