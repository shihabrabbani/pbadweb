using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Core.Filters
{
    public class EditionSearchFilter : BaseSearchFilter
    {
        public string PrivateAdType { get; set; }
        public bool IsColor { get; set; } 
        public bool Corporation { get; set; }
        public int EditionPageNo { get; set; }
    }
}
