#pragma checksum "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6f3b8269924dd7fcb73f2d25429383730826c312"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home__LoginFormPartial), @"mvc.1.0.view", @"/Views/Home/_LoginFormPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6f3b8269924dd7fcb73f2d25429383730826c312", @"/Views/Home/_LoginFormPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5ef2d37f717a2bca4e15fa34d68c1dd740ac897c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home__LoginFormPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<LoginViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<div class=\"form-container sign-in-container\">\r\n\r\n");
#nullable restore
#line 6 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml"
     using (Html.BeginForm("Login", "Account", FormMethod.Post, new { @role = "form", @id = "formLogin", @utocomplete = "off" }))
    {
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml"
   Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h1>Sign in</h1>\r\n");
#nullable restore
#line 12 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml"
   Write(Html.TextBoxFor(model => model.Username, new { @class = "form-control", @placeholder = "Username", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml"
   Write(Html.ValidationMessageFor(model => model.Username));

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div id=\"show_hide_password-1\">\r\n            ");
#nullable restore
#line 16 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml"
       Write(Html.TextBoxFor(model => model.Password, new { @class = "form-control", @type = "password", @placeholder = "Password", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            <a href=\"#\"><i class=\"fa fa-eye-slash\" aria-hidden=\"true\"></i></a>\r\n            ");
#nullable restore
#line 18 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml"
       Write(Html.ValidationMessageFor(model => model.Password));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n");
            WriteLiteral("        <a href=\"/otprequest\">Forgot your password?</a>\r\n        <button type=\"submit\" class=\"btn-login btn-teal\">Sign In</button>\r\n");
#nullable restore
#line 24 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Home\_LoginFormPartial.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
<script type=""text/javascript"">
    $(document).ready(function () {
        $(""#show_hide_password-1 a"").on('click', function (event) {
            event.preventDefault();
            if ($('#show_hide_password-1 input').attr(""type"") == ""text"") {
                $('#show_hide_password-1 input').attr('type', 'password');
                $('#show_hide_password-1 i').addClass(""fa-eye-slash"");
                $('#show_hide_password-1 i').removeClass(""fa-eye"");
            } else if ($('#show_hide_password-1 input').attr(""type"") == ""password"") {
                $('#show_hide_password-1 input').attr('type', 'text');
                $('#show_hide_password-1 i').removeClass(""fa-eye-slash"");
                $('#show_hide_password-1 i').addClass(""fa-eye"");
            }
        });
    });
</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<LoginViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
