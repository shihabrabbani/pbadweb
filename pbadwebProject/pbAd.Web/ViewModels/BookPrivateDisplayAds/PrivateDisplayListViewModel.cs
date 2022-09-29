using pbAd.Core.Filters;
using pbAd.Data.Models;
using pbAd.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.ViewModels.BookPrivateDisplayAds
{
    public class PrivateDisplayListViewModel
    {        
        public IEnumerable<ABPrintPrivateDisplay> ABPrintPrivateDisplayList { get; set; }        
        public ABPrintPrivateDisplaySearchFilter SearchFilter { get; set; }
    }
}
