#pragma checksum "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5cff01bbcafeda99fafd984171ee731d7ac40b4c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_ChangePassword), @"mvc.1.0.view", @"/Views/Account/ChangePassword.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5cff01bbcafeda99fafd984171ee731d7ac40b4c", @"/Views/Account/ChangePassword.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5ef2d37f717a2bca4e15fa34d68c1dd740ac897c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Account_ChangePassword : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ChangePasswordModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/login.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/accounts/accountManager.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
  
    ViewData["Title"] = "Change Password";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<style>
    .new-pass {
        display: block !important;
    }
    .newpassword-2nd-i.fa-eye-slash {       
        right: 14px;
        top: 20px !important;       
    }

    .cnfm-newpassword-2nd-i.fa-eye-slash {
        right: 14px;
        top: 80px !important;
    }
</style>

<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
    <strong>OTP is sent successfully to your email. Please obtain the OTP to change your password</strong>
    <button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close"">
        <span aria-hidden=""true"">&times;</span>
    </button>
</div>
<div class=""row"">
    <div class=""col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-6 offset-lg-3 col-xl-4 offset-xl-4"">
        <div class=""form-container sign-in-container"">
");
#nullable restore
#line 33 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
             using (Html.BeginForm("ChangePassword", "Account", FormMethod.Post, new { @role = "form", @class = "forgot box", @id = "formOtpGenerate", @utocomplete = "off" }))
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <h3>Change Password</h3>\r\n");
#nullable restore
#line 38 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
           Write(Html.HiddenFor(model => model.Username));

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
           Write(Html.HiddenFor(model => model.Email));

#line default
#line hidden
#nullable disable
#nullable restore
#line 41 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
           Write(Html.TextBoxFor(model => model.OTP, new { @placeholder = "OTP", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
#nullable restore
#line 42 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
           Write(Html.ValidationMessageFor(model => model.OTP));

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"new-pass\">\r\n                    <div id=\"show_hide_password-1\">\r\n                        ");
#nullable restore
#line 46 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
                   Write(Html.TextBoxFor(model => model.NewPassword, new { @placeholder = "New Password", @type = "password", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 47 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
                   Write(Html.ValidationMessageFor(model => model.NewPassword));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        <a href=\"#\"><i class=\"fa fa-eye-slash newpassword-2nd-i\" aria-hidden=\"true\"></i></a>\r\n                    </div>\r\n                    <div id=\"show_hide_password-2\">\r\n                        ");
#nullable restore
#line 51 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
                   Write(Html.TextBoxFor(model => model.ConfirmPassword, new { @placeholder = "Confirm Password", @type = "password", @autocomplete = "off" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 52 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"
                   Write(Html.ValidationMessageFor(model => model.ConfirmPassword));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                        <a href=""#""><i class=""fa fa-eye-slash eye-second cnfm-newpassword-2nd-i"" aria-hidden=""true""></i></a>
                    </div>

                    <button type=""submit"" class=""btn-login btn-teal btn-change-password"">Change Password</button>
                </div>
");
#nullable restore
#line 58 "D:\eGo\ProtidinerBangladesh\pbadweb\pbadwebProject\pbAd.Web\Views\Account\ChangePassword.cshtml"

            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "5cff01bbcafeda99fafd984171ee731d7ac40b4c11786", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5cff01bbcafeda99fafd984171ee731d7ac40b4c12965", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ChangePasswordModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
