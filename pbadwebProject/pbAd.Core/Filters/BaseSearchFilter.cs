using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Core.Filters
{
    public class BaseSearchFilter
    {
        public BaseSearchFilter()
        {
            SearchTerm = string.Empty;
            SortColumn = string.Empty;
            SortDirection = string.Empty;

            // defaults
            PageNumber = 1;
            PageSize = 20; //50
            ReturnAllRows = false;
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? SearchDate { get; set; }
        public DateTime? CreateStartDate { get; set; }
        public DateTime? CreateEndDate { get; set; }
        public string Status { get; set; }
        public string SearchTerm { get; set; }
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
        public string SearchResultsMessage { get; set; }
        public int? BookedBy { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool ReturnAllRows { get; set; }
        public bool IsSearch { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? ChangedByUserId { get; set; }
        public int? EventOwnerContactId { get; set; }
        public int? ContactId { get; set; }
        public bool IsCalculateTotal { get; set; }
        public int? AccountHeadId { get; set; }
        public int? AdvertiserId { get; set; }
        public int? BrandId { get; set; }

        public decimal PageTotal { get; set; }
        public decimal GrandTotal { get; set; }
       
    }
}
