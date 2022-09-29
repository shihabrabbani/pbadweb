#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
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

namespace pbAd.Service.ABPrintPrivateDisplays
{
    public class ABPrintPrivateDisplayService : IABPrintPrivateDisplayService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public ABPrintPrivateDisplayService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<ABPrintPrivateDisplay>> GetListByFilter(ABPrintPrivateDisplaySearchFilter filter)
        {
            var abPrintPrivateDisplayList = new List<ABPrintPrivateDisplay>();

            IQueryable<ABPrintPrivateDisplay> query = db.ABPrintPrivateDisplays
                                    .Include(f => f.BookedByUser)
                                    .Where(f =>
                                     (filter.UploadLater == false || f.UploadLater == filter.UploadLater) &&
                                     (filter.BookedBy == null || filter.BookedBy == 0 || f.BookedBy == filter.BookedBy) &&
                                     (filter.SearchTerm == string.Empty || f.BookingNo.Contains(filter.SearchTerm.Trim())));

            abPrintPrivateDisplayList = await query.ToListAsync();

            return abPrintPrivateDisplayList;
        }

        public async Task<ABPrintPrivateDisplay> GetDetailsById(int id)
        {
            var single = new ABPrintPrivateDisplay();
            try
            {
                single = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(d => d.ABPrintPrivateDisplayId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<ABPrintPrivateDisplay> GetDetailsByIdAndBookingNo(int id, string bookingNo)
        {
            var single = new ABPrintPrivateDisplay();
            try
            {
                single = await db.ABPrintPrivateDisplays
                    .Include(i => i.BookedByUser)
                    //.Include(i => i.PrivateDisplayMediaContents)
                    .FirstOrDefaultAsync(d => d.ABPrintPrivateDisplayId == id
                    && d.BookingNo == bookingNo);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<ABPrintPrivateDisplay> GetDetails(int id, string bookingNo, int bookingStep = 0, int createdBy = 0)
        {
            var single = new ABPrintPrivateDisplay();
            try
            {
                single = await db.ABPrintPrivateDisplays
                    .FirstOrDefaultAsync(d => d.ABPrintPrivateDisplayId == id
                        && d.BookingNo == bookingNo
                        && (bookingStep == 0 || (d.BookingStep == bookingStep))
                        && (createdBy == 0 || (d.CreatedBy == createdBy))
                        );
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;

        }

        public async Task<int> GetTotalCountByPaymentMode(int paymentModeId)
        {
            int totalCount = 0;
            try
            {
                totalCount = await db.ABPrintPrivateDisplays.AsNoTracking().CountAsync(d => d.PaymentModeId == paymentModeId);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return totalCount;
        }

        public async Task<IEnumerable<PrivateDisplayMediaContent>> GetMediaContentsByPrivateDisplayId(int privateDisplayId)
        {
            var filteredList = new List<PrivateDisplayMediaContent>();
            try
            {
                filteredList = await db.PrivateDisplayMediaContents.Where(d => d.ABPrintPrivateDisplayId == privateDisplayId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<PrivateDisplayMediaContent>(); ;
            }
            return filteredList;
        }

        public async Task<IEnumerable<ABPrintPrivateDisplayDetail>> GetABPrintPrivateDisplayDetailListing(int abPrintPrivateDisplayId)
        {
            var listing = new List<ABPrintPrivateDisplayDetail>();

            try
            {
                IQueryable<ABPrintPrivateDisplayDetail> query = db.ABPrintPrivateDisplayDetails.Where(f => f.ABPrintPrivateDisplayId == abPrintPrivateDisplayId);
                listing = await query.ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<ABPrintPrivateDisplayDetail>();
            }
        }

        public async Task<IEnumerable<PrivateDisplayMediaContent>> GetPrivateDisplayMediaContentListing(int abPrintPrivateDisplayId)
        {
            var listing = new List<PrivateDisplayMediaContent>();

            try
            {
                IQueryable<PrivateDisplayMediaContent> query = db.PrivateDisplayMediaContents.Where(f => f.ABPrintPrivateDisplayId == abPrintPrivateDisplayId);
                listing = await query.ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<PrivateDisplayMediaContent>();
            }
        }
        public async Task<Response<ABPrintPrivateDisplay>> BookPrivateDisplayAd(UploadLaterAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABPrintPrivateDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newABPrintPrivateDisplay = model.ABPrintPrivateDisplay;

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
                        newABPrintPrivateDisplay.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    newABPrintPrivateDisplay.CreatedDate = currentDate;

                    //let's add ab print classified display
                    await db.ABPrintPrivateDisplays.AddAsync(newABPrintPrivateDisplay);
                    await db.SaveChangesAsync();

                    model.ABPrintPrivateDisplayDetailListing.ForEach(f => f.ABPrintPrivateDisplayId = newABPrintPrivateDisplay.ABPrintPrivateDisplayId);

                    //let's add ab print classified display details
                    await db.ABPrintPrivateDisplayDetails.AddRangeAsync(model.ABPrintPrivateDisplayDetailListing);
                    await db.SaveChangesAsync();

                    if (!newABPrintPrivateDisplay.UploadLater)
                    {
                        model.PrivateDisplayMediaContents.ForEach(f => f.ABPrintPrivateDisplayId = newABPrintPrivateDisplay.ABPrintPrivateDisplayId);

                        //let's add private display media content
                        await db.PrivateDisplayMediaContents.AddRangeAsync(model.PrivateDisplayMediaContents);
                        await db.SaveChangesAsync();
                    }

                    response.Result = newABPrintPrivateDisplay;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABPrintPrivateDisplay>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
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
        public async Task<Response<ABPrintPrivateDisplay>> EditPrivateDisplayBook(UploadLaterAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABPrintPrivateDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newABPrintPrivateDisplay = model.ABPrintPrivateDisplay;

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
                        newABPrintPrivateDisplay.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    //get private display text
                    var privateDisplay = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(f =>
                                                        f.ABPrintPrivateDisplayId == model.ABPrintPrivateDisplay.ABPrintPrivateDisplayId
                                                        && f.BookingNo == model.ABPrintPrivateDisplay.BookingNo);
                    if (privateDisplay == null)
                    {
                        isOperationSuccess = false;
                        response = new Response<ABPrintPrivateDisplay>() { IsSuccess = false, Message = "Private Display not found. Please contact with support team!" };
                    }

                    //Populate to update private display
                    PopulateToUpdatePrivateDisplay(model, currentDate, privateDisplay);

                    await db.SaveChangesAsync();

                    var detailListing = await db.ABPrintPrivateDisplayDetails
                                            .Where(f => f.ABPrintPrivateDisplayId == privateDisplay.ABPrintPrivateDisplayId)
                                            .ToListAsync();

                    model.ABPrintPrivateDisplayDetailListing.ForEach(f => f.ABPrintPrivateDisplayId = newABPrintPrivateDisplay.ABPrintPrivateDisplayId);

                    //let's add ab print classified display details
                    await db.ABPrintPrivateDisplayDetails.AddRangeAsync(model.ABPrintPrivateDisplayDetailListing);
                    await db.SaveChangesAsync();

                    //reset ABPrintPrivateDisplayDetails data
                    db.ABPrintPrivateDisplayDetails.RemoveRange(detailListing);
                    await db.SaveChangesAsync();

                    if (!newABPrintPrivateDisplay.UploadLater)
                    {

                        //delete if any in remove list
                        if (!string.IsNullOrWhiteSpace(model.RemoveExistingFiles))
                        {
                            var fileNames = new List<string>();
                            var removeExistingFiles = model.RemoveExistingFiles.Split(new[] { "@@AdPro@@" }, StringSplitOptions.None);

                            foreach (var item in removeExistingFiles)
                            {
                                if (string.IsNullOrWhiteSpace(item)) continue;
                                fileNames.Add(item);
                            }

                            var removeMediaContents = await db.PrivateDisplayMediaContents
                                                  .Where(f => f.ABPrintPrivateDisplayId == privateDisplay.ABPrintPrivateDisplayId
                                                            && fileNames.Any(x => x == f.OriginalImageUrl)
                                                            )
                                                  .ToListAsync();

                            if (removeMediaContents.Any())
                            {
                                //reset media content data
                                db.PrivateDisplayMediaContents.RemoveRange(removeMediaContents);
                                await db.SaveChangesAsync();
                            }
                        }

                        model.PrivateDisplayMediaContents.ForEach(f => f.ABPrintPrivateDisplayId = newABPrintPrivateDisplay.ABPrintPrivateDisplayId);

                        //let's add private display media content
                        await db.PrivateDisplayMediaContents.AddRangeAsync(model.PrivateDisplayMediaContents);
                        await db.SaveChangesAsync();
                    }

                    if (newABPrintPrivateDisplay.UploadLater)
                    {
                        var removeMediaContents = await db.PrivateDisplayMediaContents
                                               .Where(f => f.ABPrintPrivateDisplayId == privateDisplay.ABPrintPrivateDisplayId)
                                               .ToListAsync();

                        if (removeMediaContents.Any())
                        {
                            //reset media content data
                            db.PrivateDisplayMediaContents.RemoveRange(removeMediaContents);
                            await db.SaveChangesAsync();
                        }
                    }

                    response.Result = newABPrintPrivateDisplay;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABPrintPrivateDisplay>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
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

        public async Task<Response<CheckoutPrivateDisplayProcessModel>> CheckoutPrivateDisplayDetail(CheckoutPrivateDisplayProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<CheckoutPrivateDisplayProcessModel>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    //get private display text
                    var privateDisplay = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(f =>
                                                        f.ABPrintPrivateDisplayId == model.ABPrintPrivateDisplay.ABPrintPrivateDisplayId
                                                        && f.BookingNo == model.ABPrintPrivateDisplay.BookingNo);
                    if (privateDisplay == null)
                    {
                        isOperationSuccess = false;
                        response = new Response<CheckoutPrivateDisplayProcessModel>() { IsSuccess = false, Message = "Error on Checkout process. Please contact with support team!" };
                    }

                    var updateClassifiedText = model.ABPrintPrivateDisplay;

                    privateDisplay.AdStatus = updateClassifiedText.AdStatus;
                    privateDisplay.NationalEdition = updateClassifiedText.NationalEdition;
                    privateDisplay.GrossTotal = updateClassifiedText.GrossTotal;
                    privateDisplay.DiscountPercent = updateClassifiedText.DiscountPercent;
                    privateDisplay.OfferEditionId = updateClassifiedText.OfferEditionId;
                    privateDisplay.OfferDateId = updateClassifiedText.OfferDateId;
                    privateDisplay.DiscountAmount = updateClassifiedText.DiscountAmount;
                    privateDisplay.NetAmount = updateClassifiedText.NetAmount;
                    privateDisplay.VATAmount = updateClassifiedText.VATAmount;
                    privateDisplay.Commission = updateClassifiedText.Commission;
                    privateDisplay.ApprovalStatus = updateClassifiedText.ApprovalStatus;
                    privateDisplay.PaymentModeId = updateClassifiedText.PaymentModeId;
                    privateDisplay.PaymentStatus = updateClassifiedText.PaymentStatus;
                    privateDisplay.BookingStep = updateClassifiedText.BookingStep;
                    privateDisplay.IsFixed = updateClassifiedText.IsFixed;
                    privateDisplay.CollectorId = updateClassifiedText.CollectorId;

                    privateDisplay.BillNo = updateClassifiedText.BillNo;
                    privateDisplay.BillDate = updateClassifiedText.BillDate;

                    privateDisplay.ModifiedBy = updateClassifiedText.ModifiedBy;
                    privateDisplay.ModifiedDate = currentDate;

                    await db.SaveChangesAsync();

                    var detailListing = await db.ABPrintPrivateDisplayDetails
                                            .Where(f => f.ABPrintPrivateDisplayId == model.ABPrintPrivateDisplayId)
                                            .ToListAsync();

                    var newABPrintPrivateDisplayDetailListing = new List<ABPrintPrivateDisplayDetail>();
                    foreach (var existingDetail in detailListing)
                    {
                        foreach (var newDetail in model.ABPrintPrivateDisplayDetailListing)
                        {
                            var newABPrintPrivateDisplayDetail = new ABPrintPrivateDisplayDetail
                            {
                                ABPrintPrivateDisplayId = existingDetail.ABPrintPrivateDisplayId,
                                //ContentUrl = existingDetail.ContentUrl,
                                EditionPageId = newDetail.EditionPageId,
                                EditionId = newDetail.EditionId,
                                PublishDate = existingDetail.PublishDate
                            };

                            newABPrintPrivateDisplayDetailListing.Add(newABPrintPrivateDisplayDetail);
                        }
                    }

                    //reset ABPrintPrivateDisplayDetails data
                    db.ABPrintPrivateDisplayDetails.RemoveRange(detailListing);
                    await db.SaveChangesAsync();

                    //let's add ab print private display details
                    await db.ABPrintPrivateDisplayDetails.AddRangeAsync(newABPrintPrivateDisplayDetailListing);
                    await db.SaveChangesAsync();

                    response.Result = model;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<CheckoutPrivateDisplayProcessModel>() { IsSuccess = false, Message = "Error Checkout process. Please contact with support team!" };
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

        public async Task<bool> Update(ABPrintPrivateDisplay abPrintPrivateDisplay)
        {
            try
            {
                var upateABPrintPrivateDisplay = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(d => d.ABPrintPrivateDisplayId == abPrintPrivateDisplay.ABPrintPrivateDisplayId);

                if (upateABPrintPrivateDisplay == null)
                    return false;

                upateABPrintPrivateDisplay.CategoryId = abPrintPrivateDisplay.CategoryId;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(ABPrintPrivateDisplay abPrintPrivateDisplay)
        {
            try
            {
                var removeABPrintPrivateDisplay = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(d => d.ABPrintPrivateDisplayId == abPrintPrivateDisplay.ABPrintPrivateDisplayId);

                if (removeABPrintPrivateDisplay == null)
                    return false;

                db.ABPrintPrivateDisplays.Remove(removeABPrintPrivateDisplay);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentInfo(ABPrintPrivateDisplay privateDisplay)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateABPrintPrivateDisplay = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(d => d.ABPrintPrivateDisplayId == privateDisplay.ABPrintPrivateDisplayId);

                if (updateABPrintPrivateDisplay == null)
                    return false;

                updateABPrintPrivateDisplay.PaymentType = privateDisplay.PaymentType;
                updateABPrintPrivateDisplay.PaymentMethod = privateDisplay.PaymentMethod;
                updateABPrintPrivateDisplay.PaymentMobileNumber = privateDisplay.PaymentMobileNumber;
                updateABPrintPrivateDisplay.PaymentTrxId = privateDisplay.PaymentTrxId;
                updateABPrintPrivateDisplay.PaymentPaidAmount = privateDisplay.PaymentPaidAmount;
                updateABPrintPrivateDisplay.BookingStep = privateDisplay.BookingStep;

                updateABPrintPrivateDisplay.ModifiedBy = privateDisplay.ModifiedBy;
                updateABPrintPrivateDisplay.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentMode(ABPrintPrivateDisplay aBPrintPrivateDisplay)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateABPrintPrivateDisplay = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(d => d.ABPrintPrivateDisplayId == aBPrintPrivateDisplay.ABPrintPrivateDisplayId);

                if (updateABPrintPrivateDisplay == null)
                    return false;

                updateABPrintPrivateDisplay.ApprovalStatus = aBPrintPrivateDisplay.ApprovalStatus;
                updateABPrintPrivateDisplay.PaymentStatus = aBPrintPrivateDisplay.PaymentStatus;
                updateABPrintPrivateDisplay.AdStatus = aBPrintPrivateDisplay.AdStatus;

                if (aBPrintPrivateDisplay.ActualReceived > 0)
                    updateABPrintPrivateDisplay.ActualReceived = aBPrintPrivateDisplay.ActualReceived;

                if (!string.IsNullOrWhiteSpace(aBPrintPrivateDisplay.PaymentTrxId))
                    updateABPrintPrivateDisplay.PaymentTrxId = aBPrintPrivateDisplay.PaymentTrxId;

                if (!string.IsNullOrWhiteSpace(aBPrintPrivateDisplay.MoneyReceiptNo))
                    updateABPrintPrivateDisplay.MoneyReceiptNo = aBPrintPrivateDisplay.MoneyReceiptNo;

                if (!string.IsNullOrWhiteSpace(aBPrintPrivateDisplay.PaymentMobileNumber))
                    updateABPrintPrivateDisplay.PaymentMobileNumber = aBPrintPrivateDisplay.PaymentMobileNumber;

                if (aBPrintPrivateDisplay.PaymentModeId > 0)
                    updateABPrintPrivateDisplay.PaymentModeId = aBPrintPrivateDisplay.PaymentModeId;

                updateABPrintPrivateDisplay.ModifiedBy = aBPrintPrivateDisplay.ModifiedBy;
                updateABPrintPrivateDisplay.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Response<ABPrintPrivateDisplay>> UpdateUploadLater(UploadLaterAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABPrintPrivateDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    //get private display text
                    var privateDisplay = await db.ABPrintPrivateDisplays.FirstOrDefaultAsync(f =>
                                                        f.ABPrintPrivateDisplayId == model.ABPrintPrivateDisplay.ABPrintPrivateDisplayId
                                                        && f.BookingNo == model.ABPrintPrivateDisplay.BookingNo);
                    if (privateDisplay == null)
                    {
                        isOperationSuccess = false;
                        response = new Response<ABPrintPrivateDisplay>() { IsSuccess = false, Message = "Private Display not found. Please contact with support team!" };
                    }
                    if (isOperationSuccess)
                    {
                        var updatePrivateDisplay = model.ABPrintPrivateDisplay;

                        if (updatePrivateDisplay.BillDate != null)
                            privateDisplay.BillDate = updatePrivateDisplay.BillDate;

                        privateDisplay.UploadLater = updatePrivateDisplay.UploadLater;

                        privateDisplay.UploadLaterBy = updatePrivateDisplay.UploadLaterBy;
                        privateDisplay.UploadLaterDate = updatePrivateDisplay.UploadLaterDate;

                        privateDisplay.ModifiedBy = updatePrivateDisplay.ModifiedBy;
                        privateDisplay.ModifiedDate = currentDate;

                        await db.SaveChangesAsync();

                        //let's add private display media content
                        await db.PrivateDisplayMediaContents.AddRangeAsync(model.PrivateDisplayMediaContents);
                        await db.SaveChangesAsync();

                        response.Result = model.ABPrintPrivateDisplay;
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABPrintPrivateDisplay>() { IsSuccess = false, Message = "Error upload process. Please contact with support team!" };
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
        private void PopulateToUpdatePrivateDisplay(UploadLaterAdProcessModel model, DateTime currentDate, ABPrintPrivateDisplay privateDisplay)
        {
            var updatePrivateDisplay = model.ABPrintPrivateDisplay;

            privateDisplay.PrivateAdType = updatePrivateDisplay.PrivateAdType;
            privateDisplay.IsColor = updatePrivateDisplay.IsColor;
            privateDisplay.IsGovt = updatePrivateDisplay.IsGovt;
            privateDisplay.UploadLater = updatePrivateDisplay.UploadLater;
            privateDisplay.ColumnSize = updatePrivateDisplay.ColumnSize;
            privateDisplay.InchSize = updatePrivateDisplay.InchSize;
            privateDisplay.CategoryId = updatePrivateDisplay.CategoryId;
            privateDisplay.GrossTotal = updatePrivateDisplay.GrossTotal;
            privateDisplay.DiscountPercent = updatePrivateDisplay.DiscountPercent;
            privateDisplay.OfferDateId = updatePrivateDisplay.OfferDateId;
            privateDisplay.Remarks = updatePrivateDisplay.Remarks;
            privateDisplay.UpazillaId = updatePrivateDisplay.UpazillaId;
            privateDisplay.AgencyId = updatePrivateDisplay.AgencyId;
            privateDisplay.BrandId = updatePrivateDisplay.BrandId;
            privateDisplay.AdvertiserId = updatePrivateDisplay.AdvertiserId;
            privateDisplay.InsideDhaka = updatePrivateDisplay.InsideDhaka;

            privateDisplay.ModifiedBy = updatePrivateDisplay.ModifiedBy;
            privateDisplay.ModifiedDate = currentDate;
        }
        #endregion
    }
}
