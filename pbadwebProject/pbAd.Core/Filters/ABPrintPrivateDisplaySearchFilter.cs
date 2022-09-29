using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Core.Filters
{
    public class ABPrintPrivateDisplaySearchFilter : BaseSearchFilter
    {
        public int? BookedBy { get; set; }
        public bool UploadLater { get; set; }
    }
}
