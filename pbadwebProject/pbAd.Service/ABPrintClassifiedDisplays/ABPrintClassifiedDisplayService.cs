#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.DomainModels.BookClassifiedDisplayAds;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.ABPrintClassifiedDisplays
{
    public class ABPrintClassifiedDisplayService : IABPrintClassifiedDisplayService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public ABPrintClassifiedDisplayService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<ABPrintClassifiedDisplay>> GetListByFilter(ABPrintClassifiedDisplaySearchFilter filter)
        {
            var abPrintClassfiedDisplayList = new List<ABPrintClassifiedDisplay>();

            IQueryable<ABPrintClassifiedDisplay> query = db.ABPrintClassifiedDisplays.Where(f =>
                                     (filter.SearchTerm == string.Empty || f.BookingNo.Contains(filter.SearchTerm.Trim())));

            abPrintClassfiedDisplayList = await query.AsNoTracking().ToListAsync();

            return abPrintClassfiedDisplayList;
        }

        public async Task<ABPrintClassifiedDisplay> GetDetailsByIdAndBookingNo(int id, string bookingNo, int bookingStep = 0)
        {
            var single = new ABPrintClassifiedDisplay();
            try
            {
                single = await db.ABPrintClassifiedDisplays.AsNoTracking().FirstOrDefaultAsync(d => d.ABPrintClassifiedDisplayId == id
                                && d.BookingNo == bookingNo && (bookingStep == 0 || (d.BookingStep == bookingStep)));
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
                totalCount = await db.ABPrintClassifiedDisplays.AsNoTracking().CountAsync(d => d.PaymentModeId == paymentModeId);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return totalCount;
        }

        public async Task<ABPrintClassifiedDisplay> GetDetails(int id, string bookingNo, int bookingStep = 0, int createdBy = 0)
        {
            var single = new ABPrintClassifiedDisplay();
            try
            {
                single = await db.ABPrintClassifiedDisplays
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.ABPrintClassifiedDisplayId == id
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
        public async Task<ABPrintClassifiedDisplayDetail> GetABPrintClassifiedDisplayDetail(int abPrintClassifiedDisplayId)
        {
            var single = new ABPrintClassifiedDisplayDetail();

            try
            {
                IQueryable<ABPrintClassifiedDisplayDetail> query = db.ABPrintClassifiedDisplayDetails.Where(f => f.ABPrintClassifiedDisplayId == abPrintClassifiedDisplayId);
                single = await query.AsNoTracking().FirstOrDefaultAsync();

                return single;
            }
            catch (Exception ex)
            {
                return new ABPrintClassifiedDisplayDetail();
            }
        }

        public async Task<IEnumerable<ABPrintClassifiedDisplayDetail>> GetABPrintClassifiedDisplayDetailListing(int abPrintClassifiedDisplayId)
        {
            var listing = new List<ABPrintClassifiedDisplayDetail>();

            try
            {
                IQueryable<ABPrintClassifiedDisplayDetail> query = db.ABPrintClassifiedDisplayDetails.Where(f => f.ABPrintClassifiedDisplayId == abPrintClassifiedDisplayId);
                listing = await query.AsNoTracking().ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<ABPrintClassifiedDisplayDetail>();
            }
        }

        public async Task<Response<ABPrintClassifiedDisplay>> BookClassifiedDisplayAd(BookClassifiedDisplayAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABPrintClassifiedDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newABPrintClassifiedDisplay = model.ABPrintClassifiedDisplay;

                    if (model.RequiredAdvertiser)
                    {
                        var newAdvertiser = model.Advertiser;

                        if (model.Advertiser.AdvertiserId > 0)
                        {
                            var updateAdvertiser = await db.Advertisers.FirstOrDefaultAsync(f => f.AdvertiserId == model.Advertiser.AdvertiserId);
                            if (updateAdvertiser != null)
                            {
                                updateAdvertiser.AdvertiserName = model.Advertiser.AdvertiserName;
                                updateAdvertiser.ModifiedBy = model.ABPrintClassifiedDisplay.CreatedBy;
                                updateAdvertiser.ModifiedDate = currentDate;

                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            newAdvertiser.CreatedDate = currentDate;
                            //let's add new advertiser
                            await db.Advertisers.AddAsync(newAdvertiser);
                            await db.SaveChangesAsync();
                        }

                        newABPrintClassifiedDisplay.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    newABPrintClassifiedDisplay.CreatedDate = currentDate;

                    //let's add ab print classified display
                    await db.ABPrintClassifiedDisplays.AddAsync(newABPrintClassifiedDisplay);
                    await db.SaveChangesAsync();

                    model.ABPrintClassifiedDisplayDetailListing.ForEach(f => f.ABPrintClassifiedDisplayId = newABPrintClassifiedDisplay.ABPrintClassifiedDisplayId);

                    //let's add ab print classified display details
                    await db.ABPrintClassifiedDisplayDetails.AddRangeAsync(model.ABPrintClassifiedDisplayDetailListing);
                    await db.SaveChangesAsync();

                    response.Result = newABPrintClassifiedDisplay;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABPrintClassifiedDisplay>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
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

        public async Task<Response<ABPrintClassifiedDisplay>> EditBookedClassifiedDisplayAd(BookClassifiedDisplayAdProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABPrintClassifiedDisplay>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var classifiedDisplay = model.ABPrintClassifiedDisplay;

                    if (model.RequiredAdvertiser)
                    {
                        var newAdvertiser = model.Advertiser;

                        if (model.Advertiser.AdvertiserId > 0)
                        {
                            //update
                            var updateAdvertiser = await db.Advertisers.FirstOrDefaultAsync(f => f.AdvertiserId == model.Advertiser.AdvertiserId);
                            if (updateAdvertiser != null)
                            {
                                updateAdvertiser.AdvertiserName = model.Advertiser.AdvertiserName;
                                updateAdvertiser.ModifiedBy = model.ABPrintClassifiedDisplay.CreatedBy;
                                updateAdvertiser.ModifiedDate = currentDate;

                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            //let's add new advertiser
                            await db.Advertisers.AddAsync(newAdvertiser);
                            await db.SaveChangesAsync();
                        }
                        classifiedDisplay.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    classifiedDisplay.CreatedDate = currentDate;

                    var updateClassifiedDisplay = await db.ABPrintClassifiedDisplays.FirstOrDefaultAsync(f => f.ABPrintClassifiedDisplayId == classifiedDisplay.ABPrintClassifiedDisplayId);

                    if (updateClassifiedDisplay == null)
                        isOperationSuccess = false;

                    if (isOperationSuccess)
                    {
                        //let's update ab print classified display                                       
                        PopulateToUpdateClassifiedDisplay(classifiedDisplay, updateClassifiedDisplay);
                        classifiedDisplay.ModifiedDate = currentDate;

                        await db.SaveChangesAsync();

                        var existingDetailListing = await db.ABPrintClassifiedDisplayDetails
                                        .Where(f => f.ABPrintClassifiedDisplayId == classifiedDisplay.ABPrintClassifiedDisplayId)
                                                .ToListAsync();

                        //reset ABPrintClassifiedDisplayDetails data
                        db.ABPrintClassifiedDisplayDetails.RemoveRange(existingDetailListing);
                        await db.SaveChangesAsync();

                        model.ABPrintClassifiedDisplayDetailListing.ForEach(f => f.ABPrintClassifiedDisplayId = classifiedDisplay.ABPrintClassifiedDisplayId);

                        //let's add ab print classified text details
                        await db.ABPrintClassifiedDisplayDetails.AddRangeAsync(model.ABPrintClassifiedDisplayDetailListing);
                        await db.SaveChangesAsync();

                        response.Result = classifiedDisplay;
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABPrintClassifiedDisplay>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
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

        public async Task<Response<CheckoutClassfiedDisplayProcessModel>> CheckoutClassifiedDisplayProcess(CheckoutClassfiedDisplayProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<CheckoutClassfiedDisplayProcessModel>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    //get classified display text
                    var classifiedDisplay = await db.ABPrintClassifiedDisplays.FirstOrDefaultAsync(f =>
                                                        f.ABPrintClassifiedDisplayId == model.ABPrintClassifiedDisplay.ABPrintClassifiedDisplayId
                                                        && f.BookingNo == model.ABPrintClassifiedDisplay.BookingNo);
                    if (classifiedDisplay == null)
                    {
                        isOperationSuccess = false;
                        response = new Response<CheckoutClassfiedDisplayProcessModel>() { IsSuccess = false, Message = "Error on Checkout process. Please contact with support team!" };
                    }

                    var updateClassifiedDisplay = model.ABPrintClassifiedDisplay;

                    classifiedDisplay.AdStatus = updateClassifiedDisplay.AdStatus;
                    classifiedDisplay.GrossTotal = updateClassifiedDisplay.GrossTotal;
                    classifiedDisplay.DiscountPercent = updateClassifiedDisplay.DiscountPercent;

                    classifiedDisplay.OfferEditionId = updateClassifiedDisplay.OfferEditionId;
                    classifiedDisplay.DiscountAmount = updateClassifiedDisplay.DiscountAmount;
                    classifiedDisplay.NetAmount = updateClassifiedDisplay.NetAmount;
                    classifiedDisplay.VATAmount = updateClassifiedDisplay.VATAmount;
                    classifiedDisplay.Commission = updateClassifiedDisplay.Commission;
                    classifiedDisplay.BookingStep = updateClassifiedDisplay.BookingStep;
                    classifiedDisplay.CollectorId = updateClassifiedDisplay.CollectorId;

                    classifiedDisplay.ApprovalStatus = updateClassifiedDisplay.ApprovalStatus;
                    classifiedDisplay.PaymentModeId = updateClassifiedDisplay.PaymentModeId;
                    classifiedDisplay.PaymentStatus = updateClassifiedDisplay.PaymentStatus;                    
                    classifiedDisplay.NationalEdition = updateClassifiedDisplay.NationalEdition;

                    classifiedDisplay.BillNo = updateClassifiedDisplay.BillNo;
                    classifiedDisplay.BillDate = updateClassifiedDisplay.BillDate;

                    classifiedDisplay.ModifiedBy = updateClassifiedDisplay.ModifiedBy;
                    classifiedDisplay.ModifiedDate = currentDate;
                    await db.SaveChangesAsync();

                    var detailListing = await db.ABPrintClassifiedDisplayDetails
                                            .Where(f => f.ABPrintClassifiedDisplayId == model.ABPrintClassifiedDisplayId)
                                            .ToListAsync();

                    var cdDetailListing = from cd in detailListing
                                          group cd by new
                                          {
                                              cd.ABPrintClassifiedDisplayId,
                                              cd.AdContent,
                                              cd.Title,
                                              cd.PublishDate,
                                              cd.FinalImageUrl,
                                              cd.OriginalImageUrl
                                          } into cdl
                                          select new ABPrintClassifiedDisplayDetail
                                          {
                                              ABPrintClassifiedDisplayId = cdl.Key.ABPrintClassifiedDisplayId,
                                              AdContent = cdl.Key.AdContent,
                                              Title = cdl.Key.Title,
                                              PublishDate = cdl.Key.PublishDate,
                                              FinalImageUrl = cdl.Key.FinalImageUrl,
                                              OriginalImageUrl = cdl.Key.OriginalImageUrl
                                          };

                    var newABPrintClassifiedDisplayDetailListing = new List<ABPrintClassifiedDisplayDetail>();
                    foreach (var existingDetail in cdDetailListing)
                    {
                        foreach (var newDetail in model.ABPrintClassifiedDisplayDetailListing)
                        {
                            var newABPrintClassifiedDisplayDetail = new ABPrintClassifiedDisplayDetail
                            {
                                ABPrintClassifiedDisplayId = existingDetail.ABPrintClassifiedDisplayId,
                                AdContent = existingDetail.AdContent,
                                Title = existingDetail.Title,
                                EditionId = newDetail.EditionId,
                                EditionPageId = newDetail.EditionPageId,
                                PublishDate = existingDetail.PublishDate,
                                FinalImageUrl = existingDetail.FinalImageUrl,
                                OriginalImageUrl = existingDetail.OriginalImageUrl
                            };

                            newABPrintClassifiedDisplayDetailListing.Add(newABPrintClassifiedDisplayDetail);
                        }
                    }

                    //reset ABPrintClassifiedDisplayDetails data
                    db.ABPrintClassifiedDisplayDetails.RemoveRange(detailListing);
                    await db.SaveChangesAsync();

                    //let's add ab print classified text details
                    await db.ABPrintClassifiedDisplayDetails.AddRangeAsync(newABPrintClassifiedDisplayDetailListing);
                    await db.SaveChangesAsync();

                    response.Result = model;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<CheckoutClassfiedDisplayProcessModel>() { IsSuccess = false, Message = "Error Checkout process. Please contact with support team!" };
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

        public async Task<bool> Update(ABPrintClassifiedDisplay abPrintClassfiedDisplay)
        {
            try
            {
                var upateABPrintClassifiedDisplay = await db.ABPrintClassifiedDisplays.FirstOrDefaultAsync(d => d.ABPrintClassifiedDisplayId == abPrintClassfiedDisplay.ABPrintClassifiedDisplayId);

                if (upateABPrintClassifiedDisplay == null)
                    return false;

                upateABPrintClassifiedDisplay.CategoryId = abPrintClassfiedDisplay.CategoryId;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(ABPrintClassifiedDisplay abPrintClassfiedDisplay)
        {
            try
            {
                var removeABPrintClassifiedDisplay = await db.ABPrintClassifiedDisplays.FirstOrDefaultAsync(d => d.ABPrintClassifiedDisplayId == abPrintClassfiedDisplay.ABPrintClassifiedDisplayId);

                if (removeABPrintClassifiedDisplay == null)
                    return false;

                db.ABPrintClassifiedDisplays.Remove(removeABPrintClassifiedDisplay);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentInfo(ABPrintClassifiedDisplay classifiedDisplay)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateClassifiedText = await db.ABPrintClassifiedDisplays.FirstOrDefaultAsync(d => d.ABPrintClassifiedDisplayId == classifiedDisplay.ABPrintClassifiedDisplayId);

                if (updateClassifiedText == null)
                    return false;

                updateClassifiedText.PaymentType = classifiedDisplay.PaymentType;
                updateClassifiedText.PaymentMethod = classifiedDisplay.PaymentMethod;
                updateClassifiedText.PaymentMobileNumber = classifiedDisplay.PaymentMobileNumber;
                updateClassifiedText.PaymentTrxId = classifiedDisplay.PaymentTrxId;
                updateClassifiedText.PaymentPaidAmount = classifiedDisplay.PaymentPaidAmount;
                updateClassifiedText.BookingStep = classifiedDisplay.BookingStep;

                updateClassifiedText.ModifiedBy = classifiedDisplay.ModifiedBy;
                updateClassifiedText.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentMode(ABPrintClassifiedDisplay aBPrintClassifiedDisplay)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateABPrintClassifiedDisplay = await db.ABPrintClassifiedDisplays.FirstOrDefaultAsync(d => d.ABPrintClassifiedDisplayId == aBPrintClassifiedDisplay.ABPrintClassifiedDisplayId);

                if (updateABPrintClassifiedDisplay == null)
                    return false;

                updateABPrintClassifiedDisplay.ApprovalStatus = aBPrintClassifiedDisplay.ApprovalStatus;
                updateABPrintClassifiedDisplay.PaymentStatus = aBPrintClassifiedDisplay.PaymentStatus;
                updateABPrintClassifiedDisplay.AdStatus = aBPrintClassifiedDisplay.AdStatus;

                if (aBPrintClassifiedDisplay.ActualReceived > 0)
                    updateABPrintClassifiedDisplay.ActualReceived = aBPrintClassifiedDisplay.ActualReceived;

                if (!string.IsNullOrWhiteSpace(aBPrintClassifiedDisplay.PaymentTrxId))
                    updateABPrintClassifiedDisplay.PaymentTrxId = aBPrintClassifiedDisplay.PaymentTrxId;

                if (!string.IsNullOrWhiteSpace(aBPrintClassifiedDisplay.MoneyReceiptNo))
                    updateABPrintClassifiedDisplay.MoneyReceiptNo = aBPrintClassifiedDisplay.MoneyReceiptNo;

                if (!string.IsNullOrWhiteSpace(aBPrintClassifiedDisplay.PaymentMobileNumber))
                    updateABPrintClassifiedDisplay.PaymentMobileNumber = aBPrintClassifiedDisplay.PaymentMobileNumber;

                if (aBPrintClassifiedDisplay.PaymentModeId > 0)
                    updateABPrintClassifiedDisplay.PaymentModeId = aBPrintClassifiedDisplay.PaymentModeId;

                updateABPrintClassifiedDisplay.ModifiedBy = aBPrintClassifiedDisplay.ModifiedBy;
                updateABPrintClassifiedDisplay.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Private Methods
        private void PopulateToUpdateClassifiedDisplay(ABPrintClassifiedDisplay classifiedDisplay, ABPrintClassifiedDisplay updateClassifiedDisplay)
        {
            updateClassifiedDisplay.CategoryId = classifiedDisplay.CategoryId;
            updateClassifiedDisplay.SubCategoryId = classifiedDisplay.SubCategoryId;
            updateClassifiedDisplay.TotalTitleWordCount = classifiedDisplay.TotalTitleWordCount;
            updateClassifiedDisplay.TotalAdContentWordCount = classifiedDisplay.TotalAdContentWordCount;
            updateClassifiedDisplay.AdvertiserId = classifiedDisplay.AdvertiserId;
            updateClassifiedDisplay.ActualReceived = classifiedDisplay.ActualReceived;
            updateClassifiedDisplay.GrossTotal = classifiedDisplay.GrossTotal;
            updateClassifiedDisplay.DiscountPercent = classifiedDisplay.DiscountPercent;
            updateClassifiedDisplay.DiscountAmount = classifiedDisplay.DiscountAmount;
            updateClassifiedDisplay.NetAmount = classifiedDisplay.NetAmount;
            updateClassifiedDisplay.VATAmount = classifiedDisplay.VATAmount;
            updateClassifiedDisplay.Commission = classifiedDisplay.Commission;
            updateClassifiedDisplay.OfferDateId = classifiedDisplay.OfferDateId;
            updateClassifiedDisplay.OfferEditionId = classifiedDisplay.OfferEditionId;
            updateClassifiedDisplay.UpazillaId = classifiedDisplay.UpazillaId;
            updateClassifiedDisplay.AgencyId = classifiedDisplay.AgencyId;
            updateClassifiedDisplay.BrandId = classifiedDisplay.BrandId;
            updateClassifiedDisplay.BookingStep = classifiedDisplay.BookingStep;
            updateClassifiedDisplay.ModifiedBy = classifiedDisplay.ModifiedBy;
            updateClassifiedDisplay.PaymentModeId = classifiedDisplay.PaymentModeId;
            updateClassifiedDisplay.ApprovalStatus = classifiedDisplay.ApprovalStatus;
            updateClassifiedDisplay.PaymentStatus = classifiedDisplay.PaymentStatus;
            updateClassifiedDisplay.AdStatus = classifiedDisplay.AdStatus;
            updateClassifiedDisplay.Remarks = classifiedDisplay.Remarks;
            updateClassifiedDisplay.AdColumnInch = classifiedDisplay.AdColumnInch;
            updateClassifiedDisplay.CompleteMatter = classifiedDisplay.CompleteMatter;
            updateClassifiedDisplay.NationalEdition = classifiedDisplay.NationalEdition;

        }
        #endregion
    }
}
