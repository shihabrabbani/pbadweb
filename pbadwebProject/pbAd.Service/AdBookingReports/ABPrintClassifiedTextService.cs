#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBookings;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Data.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

#endregion

namespace pbAd.Service.AdBookingReports
{
    public class AdBookingReportService : IAdBookingReportService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public AdBookingReportService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods

        public async Task<IEnumerable<BookingOrdersModel>> GetListByFilter(AdBookingReportSearchFilter filter)
        {
            var filteredList = new List<BookingOrdersModel>();
            try
            {
                var seachTerm = !string.IsNullOrWhiteSpace(filter.SearchTerm) ? $"'{filter.SearchTerm}'" : "null";
                var startDate = filter.StartDate != null ? $"'{filter.StartDate.ToNullableShortDateString()}'" : "NULL";
                var endDate = filter.EndDate != null ? $"'{filter.EndDate.ToNullableShortDateString()}'" : "NULL";

                var billStartDate = filter.BillStartDate != null ? $"'{filter.BillStartDate.ToNullableShortDateString()}'" : "NULL";
                var billEndDate = filter.BillEndDate != null ? $"'{filter.BillEndDate.ToNullableShortDateString()}'" : "NULL";

                var bookedBy = filter.BookedBy > 0 ? $"{filter.BookedBy}" : "null";

                var sqlCommand = $@"[dbo].[AdBooking_GetListingByFilter] {seachTerm},{bookedBy},{startDate},{endDate},{billStartDate},{billEndDate} ,{filter.PageNumber},{filter.PageSize}";
                filteredList = await db.Set<BookingOrdersModel>().FromSqlRaw(sqlCommand).AsNoTracking().ToListAsync();

                if (filteredList.Any())
                {
                    filter.TotalCount = filteredList[0].TotalCount;
                }

                return new StaticPagedList<BookingOrdersModel>(
                    filteredList, filter.PageNumber, filter.PageSize, filter.TotalCount);
            }
            catch (Exception ex)
            {
                return new List<BookingOrdersModel>();
            }           
        }

        public async Task<IEnumerable<UploadLaterOrdersModel>> GetUploadLatersByFilter(AdBookingReportSearchFilter filter)
        {
            var filteredList = new List<UploadLaterOrdersModel>();
            try
            {
                var seachTerm = !string.IsNullOrWhiteSpace(filter.SearchTerm) ? $"'{filter.SearchTerm}'" : "null";
                var bookedBy = filter.BookedBy>0 ? $"{filter.BookedBy}" : "null";

                var sqlCommand = $@"[dbo].[AdBooking_GetUploadLatersByFilter] {seachTerm},{bookedBy},{filter.PageNumber},{filter.PageSize}";
                filteredList = await db.Set<UploadLaterOrdersModel>().FromSqlRaw(sqlCommand).ToListAsync();

                if (filteredList.Any())
                {
                    filter.TotalCount = filteredList[0].TotalCount;
                }

                return new StaticPagedList<UploadLaterOrdersModel>(
                    filteredList, filter.PageNumber, filter.PageSize, filter.TotalCount);
            }
            catch (Exception ex)
            {
                return new List<UploadLaterOrdersModel>();
            }            
        }
        #endregion

        #region Private Methods
        
        #endregion
    }
}
