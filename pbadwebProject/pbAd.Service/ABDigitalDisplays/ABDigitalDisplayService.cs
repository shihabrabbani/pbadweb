#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.ABDigitalDisplays;
using pbAd.Data.DomainModels.ABPrintPrivateDisplays;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.ABDigitalDisplays
{
    public class ABDigitalDisplayService : IABDigitalDisplayService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public ABDigitalDisplayService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<ABDigitalDisplay>> GetListByFilter(ABDigitalDisplaySearchFilter filter)
        {
            var abDigitalDisplayList = new List<ABDigitalDisplay>();

            IQueryable<ABDigitalDisplay> query = db.ABDigitalDisplays.Where(f =>
                                     (filter.SearchTerm == string.Empty || f.BookingNo.Contains(filter.SearchTerm.Trim())));

            abDigitalDisplayList = await query.ToListAsync();

            return abDigitalDisplayList;
        }

        public async Task<ABDigitalDisplay> GetDetails(int id, string bookingno, int bookingStep = 0, int createdBy = 0)
        {
            var single = new ABDigitalDisplay();
            try
            {
                single = await db.ABDigitalDisplays.AsNoTracking().FirstOrDefaultAsync(d => d.ABDigitalDisplayId == id
                        && d.BookingNo == bookingno
                        && (bookingStep == 0 || (d.BookingStep == bookingStep))
                        && (createdBy == 0 || (d.CreatedBy == createdBy)));
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<ABDigitalDisplayDetail> GetDigitalDisplayDetailById(int digitalDisplayId)
        {
            var single = new ABDigitalDisplayDetail();
            try
            {
                single = await db.ABDigitalDisplayDetails.AsNoTracking().FirstOrDefaultAsync(d => d.ABDigitalDisplayId == digitalDisplayId);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<ABDigitalDisplay> GetDetailsByIdAndBookingNo(int id, string bookingNo)
        {
            var single = new ABDigitalDisplay();
            try
            {
                single = await db.ABDigitalDisplays
                    .Include(i => i.BookedByUser)
                    //.Include(i => i.DigitalDisplayMediaContents)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.ABDigitalDisplayId == id
                    && d.BookingNo == bookingNo);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ABDigitalDisplayDetail>> GetABDigitalDisplayDetailListing(int digitalDisplayId)
        {
            var listing = new List<ABDigitalDisplayDetail>();

            try
            {
                IQueryable<ABDigitalDisplayDetail> query = db.ABDigitalDisplayDetails
                    .Include(f => f.DigitalPage)
                    .Include(f => f.DigitalPagePosition)
                    .Include(f => f.DigitalAdUnitType)
                    .Where(f => f.ABDigitalDisplayId == digitalDisplayId);
                listing = await query.AsNoTracking().ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<ABDigitalDisplayDetail>();
            }
        }

        public async Task<IEnumerable<DigitalDisplayMediaContent>> GetDigitalDisplayMediaContentListing(int digitalDisplayId)
        {
            var listing = new List<DigitalDisplayMediaContent>();

            try
            {
                IQueryable<DigitalDisplayMediaContent> query = db.DigitalDisplayMediaContents.Where(f => f.DigitalDisplayId == digitalDisplayId);
                listing = await query.AsNoTracking().ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<DigitalDisplayMediaContent>();
            }
        }
        public async Task<int> GetTotalCountByPaymentMode(int paymentModeId)
        {
            int totalCount = 0;
            try
            {
                totalCount = await db.ABDigitalDisplays.AsNoTracking().CountAsync(d => d.PaymentModeId == paymentModeId);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return totalCount;
        }

        public async Task<bool> UpdatePaymentInfo(ABDigitalDisplay digitalDisplay)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateABDigitalDisplay = await db.ABDigitalDisplays.FirstOrDefaultAsync(d => d.ABDigitalDisplayId == digitalDisplay.ABDigitalDisplayId);

                if (updateABDigitalDisplay == null)
                    return false;

                updateABDigitalDisplay.PaymentType = digitalDisplay.PaymentType;
                updateABDigitalDisplay.PaymentMethod = digitalDisplay.PaymentMethod;
                updateABDigitalDisplay.PaymentMobileNumber = digitalDisplay.PaymentMobileNumber;
                updateABDigitalDisplay.PaymentTrxId = digitalDisplay.PaymentTrxId;
                updateABDigitalDisplay.PaymentPaidAmount = digitalDisplay.PaymentPaidAmount;
                updateABDigitalDisplay.BookingStep = digitalDisplay.BookingStep;

                updateABDigitalDisplay.ModifiedBy = digitalDisplay.ModifiedBy;
                updateABDigitalDisplay.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Response<ABDigitalDisplay>> Add(BookDigitalDisplayAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABDigitalDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newABDigitalDisplay = model.ABDigitalDisplay;

                    if (model.RequiredAdvertiser)
                    {
                        var newAdvertiser = model.Advertiser;

                        if (model.Advertiser.AdvertiserId <= 0)
                        {
                            //let's add consumer
                            await db.Advertisers.AddAsync(newAdvertiser);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            var existingAdvertiser = await db.Advertisers.FirstOrDefaultAsync(f => f.AdvertiserId == newAdvertiser.AdvertiserId);

                            if (existingAdvertiser != null)
                            {
                                existingAdvertiser.AdvertiserName = newAdvertiser.AdvertiserName;
                                existingAdvertiser.AdvertiserMobileNo = newAdvertiser.AdvertiserMobileNo;
                                await db.SaveChangesAsync();
                            }
                        }
                        newABDigitalDisplay.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    newABDigitalDisplay.CreatedDate = currentDate;

                    //let's add ab print digital display
                    await db.ABDigitalDisplays.AddAsync(newABDigitalDisplay);
                    await db.SaveChangesAsync();

                    model.ABDigitalDisplayDetailListing.ForEach(f => f.ABDigitalDisplayId = newABDigitalDisplay.ABDigitalDisplayId);

                    //let's add ab print digital display details
                    await db.ABDigitalDisplayDetails.AddRangeAsync(model.ABDigitalDisplayDetailListing);
                    await db.SaveChangesAsync();

                    if (!newABDigitalDisplay.UploadLater)
                    {
                        model.DigitalDisplayMediaContents.ForEach(f => f.DigitalDisplayId = newABDigitalDisplay.ABDigitalDisplayId);

                        //let's add private display media content
                        await db.DigitalDisplayMediaContents.AddRangeAsync(model.DigitalDisplayMediaContents);
                        await db.SaveChangesAsync();
                    }

                    response.Result = newABDigitalDisplay;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABDigitalDisplay>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
                    await ts.RollbackAsync();
                }

                if (isOperationSuccess)
                {
                    response.IsSuccess = true;
                    await ts.CommitAsync();
                }

                await ts.DisposeAsync();
            }

            return response;
        }

        public async Task<Response<CheckoutDigitalDisplayProcessModel>> AddABDigitalDisplayDetail(CheckoutDigitalDisplayProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<CheckoutDigitalDisplayProcessModel>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var detailListing = await db.ABDigitalDisplayDetails
                                            .Where(f => f.ABDigitalDisplayId == model.ABDigitalDisplayId)
                                            .ToListAsync();

                    var newABDigitalDisplayDetailListing = new List<ABDigitalDisplayDetail>();
                    foreach (var existingDetail in detailListing)
                    {
                        foreach (var newDetail in model.ABDigitalDisplayDetailListing)
                        {
                            var newABDigitalDisplayDetail = new ABDigitalDisplayDetail
                            {
                                ABDigitalDisplayId = existingDetail.ABDigitalDisplayId,
                                //ContentUrl = existingDetail.ContentUrl,
                                //ScreenType = newDetail.ScreenType
                            };

                            newABDigitalDisplayDetailListing.Add(newABDigitalDisplayDetail);
                        }
                    }

                    //reset ABDigitalDisplayDetails data
                    db.ABDigitalDisplayDetails.RemoveRange(detailListing);
                    await db.SaveChangesAsync();

                    //let's add ab print private display details
                    await db.ABDigitalDisplayDetails.AddRangeAsync(newABDigitalDisplayDetailListing);
                    await db.SaveChangesAsync();

                    response.Result = model;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<CheckoutDigitalDisplayProcessModel>() { IsSuccess = false, Message = "Error Checkout process. Please contact with support team!" };
                    await ts.RollbackAsync();
                }

                if (isOperationSuccess)
                {
                    response.IsSuccess = true;
                    await ts.CommitAsync();
                }

                await ts.DisposeAsync();
            }

            return response;
        }

        public async Task<bool> Update(ABDigitalDisplay abDigitalDisplay)
        {
            try
            {
                var upateABDigitalDisplay = await db.ABDigitalDisplays.FirstOrDefaultAsync(d => d.ABDigitalDisplayId == abDigitalDisplay.ABDigitalDisplayId);

                if (upateABDigitalDisplay == null)
                    return false;

                upateABDigitalDisplay.CategoryId = abDigitalDisplay.CategoryId;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Response<ABDigitalDisplay>> EditDigitalDisplayBook(BookDigitalDisplayAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABDigitalDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newABDigitalDisplay = model.ABDigitalDisplay;

                    if (model.RequiredAdvertiser)
                    {
                        var newAdvertiser = model.Advertiser;

                        if (model.Advertiser.AdvertiserId <= 0)
                        {
                            //let's add consumer
                            await db.Advertisers.AddAsync(newAdvertiser);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            var existingAdvertiser = await db.Advertisers.FirstOrDefaultAsync(f => f.AdvertiserId == newAdvertiser.AdvertiserId);

                            if (existingAdvertiser != null)
                            {
                                existingAdvertiser.AdvertiserName = newAdvertiser.AdvertiserName;
                                existingAdvertiser.AdvertiserMobileNo = newAdvertiser.AdvertiserMobileNo;
                                await db.SaveChangesAsync();
                            }
                        }
                        newABDigitalDisplay.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    //get digital display
                    var digitalDisplay = await db.ABDigitalDisplays.FirstOrDefaultAsync(f =>
                                                        f.ABDigitalDisplayId == model.ABDigitalDisplay.ABDigitalDisplayId
                                                        && f.BookingNo == model.ABDigitalDisplay.BookingNo);
                    if (digitalDisplay == null)
                    {
                        isOperationSuccess = false;
                        response = new Response<ABDigitalDisplay>() { IsSuccess = false, Message = "Digital Display not found. Please contact with support team!" };
                    }
                    if (isOperationSuccess)
                    {
                        //Populate to update digital display
                        PopulateToUpdateDigitalDisplay(model, currentDate, digitalDisplay);

                        await db.SaveChangesAsync();

                        var detailListing = await db.ABDigitalDisplayDetails
                                                .Where(f => f.ABDigitalDisplayId == digitalDisplay.ABDigitalDisplayId)
                                                .ToListAsync();

                        model.ABDigitalDisplayDetailListing.ForEach(f => f.ABDigitalDisplayId = newABDigitalDisplay.ABDigitalDisplayId);

                        //let's add ab print classified display details
                        await db.ABDigitalDisplayDetails.AddRangeAsync(model.ABDigitalDisplayDetailListing);
                        await db.SaveChangesAsync();

                        //reset ABDigitalDisplayDetails data
                        db.ABDigitalDisplayDetails.RemoveRange(detailListing);
                        await db.SaveChangesAsync();

                        if (!newABDigitalDisplay.UploadLater)
                        {
                            //delete if any in remove list
                            if (!string.IsNullOrWhiteSpace(model.RemoveExistingFiles))
                            {
                                var fileUrls = new List<string>();
                                var removeExistingFiles = model.RemoveExistingFiles.Split(new[] { "@@AdPro@@" }, StringSplitOptions.None);

                                foreach (var item in removeExistingFiles)
                                {
                                    if (string.IsNullOrWhiteSpace(item)) continue;
                                    fileUrls.Add(item);
                                }

                                var removeMediaContents = await db.DigitalDisplayMediaContents
                                                      .Where(f => f.DigitalDisplayId == digitalDisplay.ABDigitalDisplayId
                                                                && fileUrls.Any(x => x == f.OriginalImageUrl)
                                                                )
                                                      .ToListAsync();

                                if (removeMediaContents.Any())
                                {
                                    //reset media content data
                                    db.DigitalDisplayMediaContents.RemoveRange(removeMediaContents);
                                    await db.SaveChangesAsync();
                                }
                            }

                            model.DigitalDisplayMediaContents.ForEach(f => f.DigitalDisplayId = newABDigitalDisplay.ABDigitalDisplayId);

                            //let's add private display media content
                            await db.DigitalDisplayMediaContents.AddRangeAsync(model.DigitalDisplayMediaContents);
                            await db.SaveChangesAsync();

                            var sqlCommand = $@"UPDATE [dbo].[DigitalDisplayMediaContent] SET
                                            ScreenType='{model.ScreenType}' WHERE DigitalDisplayId={newABDigitalDisplay.ABDigitalDisplayId}";
                            await db.Database.ExecuteSqlCommandAsync(sqlCommand);
                        }

                        if (newABDigitalDisplay.UploadLater)
                        {
                            var removeMediaContents = await db.DigitalDisplayMediaContents
                                                   .Where(f => f.DigitalDisplayId == digitalDisplay.ABDigitalDisplayId)
                                                   .ToListAsync();

                            if (removeMediaContents.Any())
                            {
                                //reset media content data
                                db.DigitalDisplayMediaContents.RemoveRange(removeMediaContents);
                                await db.SaveChangesAsync();
                            }
                        }

                        response.Result = newABDigitalDisplay;
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABDigitalDisplay>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
                    await ts.RollbackAsync();
                }

                if (isOperationSuccess)
                {
                    response.IsSuccess = true;
                    await ts.CommitAsync();
                }

                await ts.DisposeAsync();
            }

            return response;
        }

        public async Task<bool> CheckoutDigitalDisplay(ABDigitalDisplay abDigitalDisplay)
        {
            try
            {
                var currentDate = DateTime.Now;

                var upateABDigitalDisplay = await db.ABDigitalDisplays.FirstOrDefaultAsync(d => d.ABDigitalDisplayId == abDigitalDisplay.ABDigitalDisplayId);

                if (upateABDigitalDisplay == null)
                    return false;

                upateABDigitalDisplay.ABDigitalDisplayId = abDigitalDisplay.ABDigitalDisplayId;
                upateABDigitalDisplay.BookingNo = abDigitalDisplay.BookingNo;
                upateABDigitalDisplay.BookingStep = abDigitalDisplay.BookingStep;
                upateABDigitalDisplay.AdStatus = abDigitalDisplay.AdStatus;
                upateABDigitalDisplay.GrossTotal = abDigitalDisplay.GrossTotal;
                upateABDigitalDisplay.DiscountPercent = abDigitalDisplay.DiscountPercent;
                upateABDigitalDisplay.PaymentModeId = abDigitalDisplay.PaymentModeId;
                upateABDigitalDisplay.PaymentStatus = abDigitalDisplay.PaymentStatus;
                upateABDigitalDisplay.IsFixed = abDigitalDisplay.IsFixed;
                upateABDigitalDisplay.BillNo = abDigitalDisplay.BillNo;
                upateABDigitalDisplay.BillDate = abDigitalDisplay.BillDate;
                upateABDigitalDisplay.DiscountAmount = abDigitalDisplay.DiscountAmount;
                upateABDigitalDisplay.NetAmount = abDigitalDisplay.NetAmount;
                upateABDigitalDisplay.Commission = abDigitalDisplay.Commission;
                upateABDigitalDisplay.VATAmount = abDigitalDisplay.VATAmount;
                upateABDigitalDisplay.ApprovalStatus = abDigitalDisplay.ApprovalStatus;
                upateABDigitalDisplay.CollectorId = abDigitalDisplay.CollectorId;

                upateABDigitalDisplay.ModifiedBy = abDigitalDisplay.ModifiedBy;
                upateABDigitalDisplay.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(ABDigitalDisplay abDigitalDisplay)
        {
            try
            {
                var removeABDigitalDisplay = await db.ABDigitalDisplays.FirstOrDefaultAsync(d => d.ABDigitalDisplayId == abDigitalDisplay.ABDigitalDisplayId);

                if (removeABDigitalDisplay == null)
                    return false;

                db.ABDigitalDisplays.Remove(removeABDigitalDisplay);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentMode(ABDigitalDisplay digitalDisplay)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateABDigitalDisplay = await db.ABDigitalDisplays.FirstOrDefaultAsync(d => d.ABDigitalDisplayId == digitalDisplay.ABDigitalDisplayId);

                if (updateABDigitalDisplay == null)
                    return false;

                updateABDigitalDisplay.ApprovalStatus = digitalDisplay.ApprovalStatus;
                updateABDigitalDisplay.PaymentStatus = digitalDisplay.PaymentStatus;
                updateABDigitalDisplay.AdStatus = digitalDisplay.AdStatus;

                if (digitalDisplay.ActualReceived > 0)
                    updateABDigitalDisplay.ActualReceived = digitalDisplay.ActualReceived;

                if (!string.IsNullOrWhiteSpace(digitalDisplay.PaymentTrxId))
                    updateABDigitalDisplay.PaymentTrxId = digitalDisplay.PaymentTrxId;

                if (!string.IsNullOrWhiteSpace(digitalDisplay.MoneyReceiptNo))
                    updateABDigitalDisplay.MoneyReceiptNo = digitalDisplay.MoneyReceiptNo;

                if (!string.IsNullOrWhiteSpace(digitalDisplay.PaymentMobileNumber))
                    updateABDigitalDisplay.PaymentMobileNumber = digitalDisplay.PaymentMobileNumber;

                if (digitalDisplay.PaymentModeId > 0)
                    updateABDigitalDisplay.PaymentModeId = digitalDisplay.PaymentModeId;

                updateABDigitalDisplay.ModifiedBy = digitalDisplay.ModifiedBy;
                updateABDigitalDisplay.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Response<ABDigitalDisplay>> UpdateUploadLater(UploadLaterAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABDigitalDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    //get digital display
                    var digitalDisplay = await db.ABDigitalDisplays.FirstOrDefaultAsync(f =>
                                                        f.ABDigitalDisplayId == model.ABDigitalDisplay.ABDigitalDisplayId
                                                        && f.BookingNo == model.ABDigitalDisplay.BookingNo);
                    if (digitalDisplay == null)
                    {
                        isOperationSuccess = false;
                        response = new Response<ABDigitalDisplay>() { IsSuccess = false, Message = "Digital Display not found. Please contact with support team!" };
                    }
                    if (isOperationSuccess)
                    {
                        var updatePrivateDisplay = model.ABDigitalDisplay;

                        if (updatePrivateDisplay.BillDate != null)
                            digitalDisplay.BillDate = updatePrivateDisplay.BillDate;

                        digitalDisplay.UploadLater = updatePrivateDisplay.UploadLater;
                        digitalDisplay.UploadLaterBy = updatePrivateDisplay.UploadLaterBy;
                        digitalDisplay.UploadLaterDate = updatePrivateDisplay.UploadLaterDate;

                        digitalDisplay.ModifiedBy = updatePrivateDisplay.ModifiedBy;
                        digitalDisplay.ModifiedDate = currentDate;

                        await db.SaveChangesAsync();

                        //let's add digital display media content
                        await db.DigitalDisplayMediaContents.AddRangeAsync(model.DigitalDisplayMediaContents);
                        await db.SaveChangesAsync();

                        response.Result = model.ABDigitalDisplay;
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABDigitalDisplay>() { IsSuccess = false, Message = "Error upload process. Please contact with support team!" };
                    await ts.RollbackAsync();
                }

                if (isOperationSuccess)
                {
                    response.IsSuccess = true;
                    await ts.CommitAsync();
                }

                await ts.DisposeAsync();
            }

            return response;
        }

        #endregion

        #region Private Methods

        private void PopulateToUpdateDigitalDisplay(BookDigitalDisplayAdProcessModel model, DateTime currentDate, ABDigitalDisplay digitalDisplay)
        {
            var updateDigitalDisplay = model.ABDigitalDisplay;

            digitalDisplay.UploadLater = updateDigitalDisplay.UploadLater;

            digitalDisplay.CategoryId = updateDigitalDisplay.CategoryId;

            digitalDisplay.DiscountPercent = updateDigitalDisplay.DiscountPercent;
            //digitalDisplay.OfferDateId = updateDigitalDisplay.OfferDateId;
            digitalDisplay.Remarks = updateDigitalDisplay.Remarks;
            digitalDisplay.UpazillaId = updateDigitalDisplay.UpazillaId;
            digitalDisplay.AgencyId = updateDigitalDisplay.AgencyId;
            digitalDisplay.BrandId = updateDigitalDisplay.BrandId;
            digitalDisplay.AdvertiserId = updateDigitalDisplay.AdvertiserId;
            digitalDisplay.InsideDhaka = updateDigitalDisplay.InsideDhaka;

            digitalDisplay.GrossTotal = updateDigitalDisplay.GrossTotal;
            digitalDisplay.DiscountAmount = updateDigitalDisplay.DiscountAmount;
            digitalDisplay.NetAmount = updateDigitalDisplay.NetAmount;
            digitalDisplay.VATAmount = updateDigitalDisplay.VATAmount;
            digitalDisplay.Commission = updateDigitalDisplay.Commission;

            digitalDisplay.TotalQty = updateDigitalDisplay.TotalQty;

            digitalDisplay.ModifiedBy = updateDigitalDisplay.ModifiedBy;
            digitalDisplay.ModifiedDate = currentDate;
        }

        #endregion
    }
}
