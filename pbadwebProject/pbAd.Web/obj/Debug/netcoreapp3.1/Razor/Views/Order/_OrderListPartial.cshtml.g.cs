#pragma checksum "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e945a31f583f7aea9f78b931c1902d0cd56f73f0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Order__OrderListPartial), @"mvc.1.0.view", @"/Views/Order/_OrderListPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e945a31f583f7aea9f78b931c1902d0cd56f73f0", @"/Views/Order/_OrderListPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5ef2d37f717a2bca4e15fa34d68c1dd740ac897c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Order__OrderListPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AdBookingOrderListViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-12 x-scroller\">\r\n");
#nullable restore
#line 6 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
         if (Model.BookingOrderList.Any())
        {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            <table class=""table table-hover table-striped table-bordered"">
                <thead class=""thead-light"">
                    <tr>
                        <th scope=""col"">Booking #</th>
                        <th scope=""col"">Ad Type</th>
                        <th scope=""col"">Booked By</th>
                        <th scope=""col"">Booked On</th>
                        <th scope=""col"">Bill On</th>
                        <th scope=""col"">Agency Name</th>
                        <th scope=""col"">Brand Name</th>
                        <th scope=""col"">Avertiser</th>
                        <th scope=""col"">Avertiser Mobile</th>
                        <th scope=""col"">Bill No</th>
                        <th scope=""col"">Net Payable</th>
                    </tr>
                </thead>
                <tbody>
");
#nullable restore
#line 25 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                     foreach (var item in Model.BookingOrderList)
                    {
                        var billDate = "";
                        if (item.BillDate != null)
                        {
                            billDate = ((DateTime)item.BillDate).ToString("dd-MMM-yyyy h:mm tt");
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <th scope=\"row\">");
#nullable restore
#line 33 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                                       Write(item.BookingNo);

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                            <td>");
#nullable restore
#line 34 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.AdType);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 35 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.BookedByUser);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 36 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.BookDate.ToString("dd-MMM-yyyy h:mm tt"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 37 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(billDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 38 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.AgencyName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 39 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.BrandName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 40 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.AdvertiserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 41 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.AdvertiserMobileNo);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>\r\n");
#nullable restore
#line 43 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                                 if (!string.IsNullOrWhiteSpace(item.BillNo))
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <a target=\"_blank\"");
            BeginWriteAttribute("href", " href=\"", 2110, "\"", 2130, 1);
#nullable restore
#line 45 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
WriteAttributeValue("", 2117, item.BillURL, 2117, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 45 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                                                                       Write(item.BillNo);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n");
#nullable restore
#line 46 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            </td>\r\n                            <td>");
#nullable restore
#line 48 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                           Write(item.NetPayable);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        </tr>\r\n");
#nullable restore
#line 50 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </tbody>\r\n            </table>\r\n");
#nullable restore
#line 54 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
             if (Model.SearchFilter.TotalCount > Model.SearchFilter.PageSize)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"pagination__wrapper add_bottom_30\">\r\n                    ");
#nullable restore
#line 57 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
               Write(Html.PagedListPager((IPagedList)Model.BookingOrderList, page => Url.Action("List", new
                   {
                       pageNumber = page,
                       searchTerm = Model.SearchFilter.SearchTerm,
                       startDate = Model.SearchFilter.StartDate.ToNullableShortDateStringOrNull(),
                       endDate = Model.SearchFilter.EndDate.ToNullableShortDateStringOrNull(),
                       billStartDate = Model.SearchFilter.BillStartDate.ToNullableShortDateStringOrNull(),
                       billEndDate = Model.SearchFilter.BillEndDate.ToNullableShortDateStringOrNull(),
                       sort = Model.SearchFilter.SortColumn,
                       sortdir = Model.SearchFilter.SortDirection
                   })));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n");
#nullable restore
#line 69 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 69 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
             
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"alert alert-dark\" role=\"alert\">\r\n                There are no recods to show\r\n            </div>\r\n");
#nullable restore
#line 76 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Order\_OrderListPartial.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>");
        }
        #pragma warning restore 1998
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AdBookingOrderListViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
