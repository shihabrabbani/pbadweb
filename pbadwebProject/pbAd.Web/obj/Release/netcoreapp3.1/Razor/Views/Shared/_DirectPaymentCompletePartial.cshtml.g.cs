#pragma checksum "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ea1b43c7c89f85ed626f72a4ad8eda25f7780b1b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__DirectPaymentCompletePartial), @"mvc.1.0.view", @"/Views/Shared/_DirectPaymentCompletePartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ea1b43c7c89f85ed626f72a4ad8eda25f7780b1b", @"/Views/Shared/_DirectPaymentCompletePartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5ef2d37f717a2bca4e15fa34d68c1dd740ac897c", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__DirectPaymentCompletePartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<pbAd.Web.ViewModels.Checkout.PaymentViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<div class=\"row\">\r\n    <div class=\"col-md-4\">\r\n        <div class=\"form-group\">\r\n            <div class=\"card\">\r\n                <img class=\"card-img-top logo-direct-payment\"");
            BeginWriteAttribute("PaymentMethod", "\r\n                     PaymentMethod=\"", 232, "\"", 299, 1);
#nullable restore
#line 9 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
WriteAttributeValue("", 270, PaymentMethodConstants.bKash, 270, 29, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("\r\n                     src=\"/img/logo-bKash.png\" alt=\"Card image cap\">\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
            WriteLiteral("    <div class=\"col-md-4\">\r\n        <div class=\"form-group\">\r\n            <div class=\"card\">\r\n                <img class=\"card-img-top logo-direct-payment\"");
            BeginWriteAttribute("PaymentMethod", "\r\n                     PaymentMethod=\"", 924, "\"", 991, 1);
#nullable restore
#line 27 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
WriteAttributeValue("", 962, PaymentMethodConstants.Nogod, 962, 29, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@"
                     src=""/img/logo-nogod.png"" alt=""Card image cap"">
            </div>
        </div>
    </div>
</div>

<div class=""row"">
    <div class=""col-md-12"">
        <div class=""alert alert-success"" role=""alert"">
            <p>
                <i class=""fa fa-bullhorn""></i> Please click any one of the above payment method like bKash, Rocket or Nogod. Fill the below information like mobile number that you use to send money using above payment method and then enter amount.
                You will have a successfull message from bKash or Rocket or Nogod  with Transaction ID (Trx ID) and have to enter this Trx Id.
                Finally Hit Submit button to complete your payment.
            </p>
        </div>
    </div>
</div>
<div class=""row"">
    <div class=""col-md-6"">
        <div class=""form-group"">
            ");
#nullable restore
#line 48 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.LabelFor(model => model.PaymentMobileNumber));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 49 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.TextBoxFor(model => model.PaymentMobileNumber, new { @class = "form-control", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 50 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.ValidationMessageFor(model => model.PaymentMobileNumber));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6\">\r\n        <div class=\"form-group\">\r\n            ");
#nullable restore
#line 55 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.LabelFor(model => model.NetPayable));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 56 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.TextBoxFor(model => model.NetPayable, new { @class = "form-control", @readonly = "readonly", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 57 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.ValidationMessageFor(model => model.NetPayable));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n</div>\r\n<div class=\"row\">\r\n    <div class=\"col-md-6\">\r\n        <div class=\"form-group\">\r\n            ");
#nullable restore
#line 64 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.LabelFor(model => model.PaymentPaidAmount));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 65 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.TextBoxFor(model => model.PaymentPaidAmount, new { @class = "form-control", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 66 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.ValidationMessageFor(model => model.PaymentPaidAmount));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n    <div class=\"col-md-6\">\r\n        <div class=\"form-group\">\r\n            ");
#nullable restore
#line 71 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.LabelFor(model => model.PaymentTrxId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 72 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.TextBoxFor(model => model.PaymentTrxId, new { @class = "form-control", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 73 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Shared\_DirectPaymentCompletePartial.cshtml"
       Write(Html.ValidationMessageFor(model => model.PaymentTrxId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<pbAd.Web.ViewModels.Checkout.PaymentViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
