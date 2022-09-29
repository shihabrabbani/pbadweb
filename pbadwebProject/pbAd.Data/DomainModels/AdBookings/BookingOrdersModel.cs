using pbAd.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.AdBookings
{
    public class BookingOrdersModel
    {
        public string AdType { get; set; }
        public string AdTypeDisplayName { get; set; }
        public int AutoId { get; set; }
        public string BookingNo { get; set; }
        public DateTime BookDate { get; set; }
        public string AdvertiserName { get; set; }
        public string AdvertiserMobileNo { get; set; }
        public string AgencyName { get; set; }
        public string BillNo { get; set; }
        public string BillURL { get; set; }
        public DateTime? BillDate { get; set; }
        public string BrandName { get; set; }
        public string BookedByUser { get; set; }
        public int NetPayable { get; set; }
        public int TotalCount { get; set; }
    }

    public class UploadLaterOrdersModel
    {
        public string AdType { get; set; }
        public string AdTypeDisplayName { get; set; }
        public int AutoId { get; set; }
        public string BookingNo { get; set; }
        public DateTime BookDate { get; set; }        
        public string BookedBy { get; set; }
        public string BillNo { get; set; }
        public string BillURL { get; set; }
        public int NetPayable { get; set; }
        public int TotalCount { get; set; }
    }
}
