#pragma checksum "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9dcacc5682a381c506cd02ea277dfc4b74a9284f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_BookGovtPrivateDisplayAd_BookNow), @"mvc.1.0.view", @"/Views/BookGovtPrivateDisplayAd/BookNow.cshtml")]
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
#line 1 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.Account;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.RndContents;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.AddBooks;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Core.Utilities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.Infrastructure.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Core.Filters;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.ViewModels.AdBookingReports;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using X.PagedList;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using X.PagedList.Mvc.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Web.Infrastructure.Framework;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Service.CacheManagerServices;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\_ViewImports.cshtml"
using pbAd.Data.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9dcacc5682a381c506cd02ea277dfc4b74a9284f", @"/Views/BookGovtPrivateDisplayAd/BookNow.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5ef2d37f717a2bca4e15fa34d68c1dd740ac897c", @"/Views/_ViewImports.cshtml")]
    public class Views_BookGovtPrivateDisplayAd_BookNow : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<pbAd.Web.ViewModels.BookPrivateDisplayAds.BookPrivateDisplayGovtAdViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#nullable restore
#line 4 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
  
    ViewData["Title"] = "Book Govt Ad";
    Layout = "~/Views/Shared/_Layout.cshtml";
    var version = Configuration.GetSection("Proshar")["Version"];

    var userGroup = User.Claims.FirstOrDefault(c => c.Type == "GroupId").Value;
    var canViewCRM = userGroup == UserGroupConstants.CRM_User.ToString();

    var toggleShowHide = canViewCRM ? "d-none" : "";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("BookingWidget", async() => {
                WriteLiteral(@"
    <div class=""steps"">
        <ul>
            <li class=""active"">
                <span>1</span>
                Start
            </li>
            <li class=""active"">
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
            WriteLiteral("\r\n<div class=\"form-wizard\">\r\n    <div class=\"myContainer\">\r\n");
#nullable restore
#line 36 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
         using (Html.BeginForm("booknow", "bookgovtprivatedisplayad", FormMethod.Post, new { @role = "form", @class = "form-container", @id = "form-book-govt-ad" }))
        {
            

#line default
#line hidden
#nullable disable
#nullable restore
#line 38 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
       Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
       Write(Html.HiddenFor(model => model.EstimatedTotal));

#line default
#line hidden
#nullable disable
#nullable restore
#line 40 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
       Write(Html.HiddenFor(model => model.DatesBasedOffer));

#line default
#line hidden
#nullable disable
#nullable restore
#line 41 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
       Write(Html.Hidden("HiddenAdvertiserName", Model.AdvertiserName));

#line default
#line hidden
#nullable disable
#nullable restore
#line 42 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
       Write(Html.Hidden("UserGroup", userGroup));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            <div class=""row"">
                <div class=""col-md-9"">
                    <div class=""main-grid compose"">
                        <div class=""title_bg"">
                            <div class=""cat-select contact"">
                                <div class=""row"">
                                    <div class=""col-md-6"">
                                        <div class=""form-group"">
                                            <div class=""custom-control custom-checkbox"">
                                                ");
#nullable restore
#line 53 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                           Write(Html.CheckBoxFor(model => model.Corporation, new { @class = "custom-control-input", @value = "true" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                <label class=""custom-control-label"" for=""Corporation"">Corporation</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class=""col-md-6"">
                                        <div class=""form-group"">
                                            <div class=""custom-control custom-checkbox"">
                                                ");
#nullable restore
#line 61 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                           Write(Html.CheckBoxFor(model => model.IsColor, new { @class = "custom-control-input", @value = "true" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                <label class=""custom-control-label"" for=""IsColor"">Color</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class=""row"">
                                    <div class=""col-md-6"">
                                        <div class=""form-group"">
                                            ");
#nullable restore
#line 71 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.DropDownListFor(m => m.EditionPageId, Model.EditionPageDropdownList, "Select Edition Page", new { @class = "form-control", @placeholder = "Edition Page" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 72 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.ValidationMessageFor(model => model.EditionPageId));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </div>
                                    </div>

                                    <div class=""col-md-6"">
                                        <div class=""form-group"">
                                            ");
#nullable restore
#line 78 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.DropDownListFor(m => m.ColumnSize, Model.ColumnSizeDropdownList, "Select Column Size", new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 79 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.ValidationMessageFor(model => model.ColumnSize));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </div>
                                    </div>

                                </div>

                                <div class=""row"">

                                    <div class=""col-md-6"">
                                        <div class=""form-group"">
                                            ");
#nullable restore
#line 89 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.LabelFor(model => model.InchSize));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 90 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.TextBoxFor(model => model.InchSize, new { @class = "form-control", @autocomplete = "off", @placeholder = "Inch Size" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 91 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.ValidationMessageFor(model => model.InchSize));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </div>
                                    </div>
                                    <div class=""col-md-6"">
                                        <label>Brand <i class=""fa fa-info-circle"" data-toggle=""tooltip"" title=""Type minimun 2 characters for brand name""></i></label>
                                        ");
#nullable restore
#line 96 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.TextBoxFor(model => model.BrandAutoComplete, new { @class = "form-control", @placeholder = "Type Brand" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 97 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.HiddenFor(model => model.BrandId));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                    </div>
                                </div>

                                <div class=""row"">
                                    <div class=""col-md-6"">
                                        <div class=""form-group"">
                                            ");
#nullable restore
#line 104 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.LabelFor(model => model.CategoryId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 105 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.DropDownListFor(m => m.CategoryId, new List<SelectListItem>(), "Select Category", new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 106 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.ValidationMessageFor(model => model.CategoryId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </div>\r\n                                    </div>\r\n");
#nullable restore
#line 109 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                     if (canViewCRM)
                                    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                        <div class=""col-md-6"">
                                            <label>Agency <i class=""fa fa-info-circle"" data-toggle=""tooltip"" title=""Type minimun 2 characters for agency""></i></label>
                                            ");
#nullable restore
#line 113 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.TextBoxFor(model => model.AgencyAutoComplete, new { @class = "form-control", @placeHolder = "Type Agency" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 114 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.HiddenFor(model => model.AgencyId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </div>\r\n");
#nullable restore
#line 116 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    <div class=\"col-6\">\r\n                                        ");
#nullable restore
#line 119 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.LabelFor(model => model.AdvertiserName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 120 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.TextBoxFor(model => model.AdvertiserName, new { @class = "form-control", autocomplete = "off", @placeholder = "Advertiser Name" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 121 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.ValidationMessageFor(model => model.AdvertiserName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 122 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.HiddenFor(model => model.AdvertiserId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </div>\r\n                                    <div class=\"col-6\">\r\n                                        ");
#nullable restore
#line 125 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.LabelFor(model => model.AdvertiserContactNumber));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 126 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.TextBoxFor(model => model.AdvertiserContactNumber, new { @class = "form-control", autocomplete = "off", @placeholder = "Contact Number" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 127 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.ValidationMessageFor(model => model.AdvertiserContactNumber));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        ");
#nullable restore
#line 128 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                   Write(Html.HiddenFor(model => model.AdvertiserContactNumber));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </div>\r\n\r\n");
            WriteLiteral(@"                                </div>



                                <div class=""row"">
                                    <div class=""col-md-12"">
                                        <div class=""form-group"">
                                            ");
#nullable restore
#line 152 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.LabelFor(model => model.Remarks));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 153 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.TextAreaFor(model => model.Remarks, new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                            ");
#nullable restore
#line 154 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                       Write(Html.ValidationMessageFor(model => model.Remarks));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </div>\r\n                                    </div>\r\n                                </div>\r\n");
#nullable restore
#line 158 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                 if (canViewCRM)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                    <div class=""row"">
                                        <div class=""col-md-6"">
                                            <div class=""form-group"">
                                                <div class=""custom-control custom-checkbox"">
                                                    ");
#nullable restore
#line 164 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                               Write(Html.CheckBoxFor(model => model.InsideDhaka, new { @class = "custom-control-input", @value = "true" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                    <label class=""custom-control-label"" for=""InsideDhaka"">Inside Dhaka</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
");
#nullable restore
#line 170 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                }
                                else
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                    <div class=""row d-none"">
                                        <div class=""col-md-6"">
                                            <div class=""form-group"">
                                                <div class=""custom-control custom-checkbox"">
                                                    ");
#nullable restore
#line 177 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                               Write(Html.CheckBoxFor(model => model.InsideDhaka, new { @class = "custom-control-input", @value = "true", @checked = "checked" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                    <label class=""custom-control-label"" for=""InsideDhaka"">Inside Dhaka</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
");
#nullable restore
#line 183 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                            </div>
                        </div>
                    </div>

                    <div class=""main-grid compose"">
                        <div class=""title_bg"">
                            <h5>Upload Files</h5>
                            <div class=""cat-select contact"">
                                <div class=""col-md-12"">
                                    <div class=""row"">
                                        <div class=""row col-md-5"">
                                            <span style=""padding-bottom: 0px"" class=""btn btn-teal section-add-files"">
                                                <i class=""fa fa-plus add-icon""></i>
                                                <label for=""ImageContents"">Add files...</label>
                                                <input hidden type=""file"" id=""ImageContents"" name=""ImageContents"" multiple onchange=""bookGovtAdManager.previewPhoto()"" />
                                            </span>
       ");
            WriteLiteral(@"                                 </div>
                                        <div class=""col-md-6"">
                                            <div class=""form-group"">
                                                <div class=""custom-control custom-checkbox"">
                                                    <input type=""checkbox"" class=""custom-control-input"" id=""UploadLater"" name=""UploadLater"" value=""true"" />
                                                    <label class=""custom-control-label"" for=""UploadLater"">Upload Later</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""overflow-auto mt-1"">
                                    <!-- The table listing the files available for upload/download -->
                                    <table class=""table tabl");
            WriteLiteral(@"e-striped tbl-uploded-files"">
                                        <tbody class=""uploaded-files"">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=""col-md-3"">
                    <div class=""btn btn-teal compose-ad"">Summary</div>
                    <div class=""add-preview"">
                        <div class=""bdr"">
                            <div class=""con cost""");
            BeginWriteAttribute("id", " id=\"", 14045, "\"", 14050, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                                <div class=""ad-preview"">
                                    <table class=""table"">
                                        <tbody>
                                            <tr>
                                                <td class=""td-top"">Column Size:</td>
                                                <td class=""column-size td-top text-right"">0</td>
                                            </tr>
                                            <tr>
                                                <td>Inch Size:</td>
                                                <td class=""inch-size text-right"">0</td>
                                            </tr>
                                            <tr>
                                                <td>Rate:</td>
                                                <td class=""rate-per-column-inch text-right"">0</td>
                                            </tr>
                                            <t");
            WriteLiteral(@"r>
                                                <td class=""td-bottom"">Estimated Cost [BDT]:</td>
                                                <td class=""esitmated-cost-amount td-bottom text-right"">0</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class=""add-preview"">
                        <div class=""btn btn-teal compose-ad"">Date Based Offer</div>
                        <div class=""bdr"">
                            <div class=""rate"" id=""rate"">
                                <!--Add here date picker -->
                                <div class=""input-group input-dateranged"">
                                    <input type=""text"" autocomplete=""off"" class=""form-control prs-datepicker"" id=""DateBasedOffer"" placeholder=""Select Date""");
            WriteLiteral(@">
                                    <button class=""btn btn-teal btn-date-based-offer"" type=""button"" style=""margin:0px 10px;color:white;"">ADD</button>
                                </div>
                                <div class=""date-box date-based-offer-container mt-1"">
                                    <span><i class=""fa fa-exclamation-triangle"" aria-hidden=""true""></i> No dates selected</span>
                                </div>
                                <hr />
                                <div class=""date-box"">
                                    <p>Discount: <span class=""discount-offer"">0</span>%</p>
                                </div>
                            </div>
                        </div>
                        <button type=""submit"" class=""btn btn-teal btn-make-payment"" style=""margin:10px 0px;"">MAKE PAYMENT <i class=""fa fa-long-arrow-right""></i></button>
                    </div>

                </div>

            </div>
");
#nullable restore
#line 277 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<!--  -->\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "9dcacc5682a381c506cd02ea277dfc4b74a9284f33363", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "href", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 17209, "~/css/adbook.css?v=", 17209, 19, true);
#nullable restore
#line 284 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
AddHtmlAttributeValue("", 17228, version, 17228, 8, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9dcacc5682a381c506cd02ea277dfc4b74a9284f35124", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 17257, "~/js/book-govt-private-display-ads/bookGovtAdManager.js?v=", 17257, 58, true);
#nullable restore
#line 285 "D:\eGo\ProtidinerBangladesh\AdWeb\pbadwebProject\pbAd.Web\Views\BookGovtPrivateDisplayAd\BookNow.cshtml"
AddHtmlAttributeValue("", 17315, version, 17315, 8, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
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
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IConfiguration Configuration { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<pbAd.Web.ViewModels.BookPrivateDisplayAds.BookPrivateDisplayGovtAdViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
