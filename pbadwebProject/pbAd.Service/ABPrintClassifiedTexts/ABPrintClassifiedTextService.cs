#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.DomainModels.AdBooks;
using pbAd.Data.DomainModels.Checkouts;
using pbAd.Data.Models;
using pbAd.Data.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.ABPrintClassifiedTexts
{
    public class ABPrintClassifiedTextService : IABPrintClassifiedTextService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public ABPrintClassifiedTextService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<ABPrintClassifiedText>> GetListByFilter(ABPrintClassifiedTextSearchFilter filter)
        {
            var abPrintClassifiedTextList = new List<ABPrintClassifiedText>();

            IQueryable<ABPrintClassifiedText> query = db.ABPrintClassifiedTexts.Where(f =>
                                     (filter.SearchTerm == string.Empty || f.BookingNo.Contains(filter.SearchTerm.Trim())));

            abPrintClassifiedTextList = await query.AsNoTracking().ToListAsync();

            return abPrintClassifiedTextList;
        }

        public async Task<ABPrintClassifiedText> GetDetailsById(int id)
        {
            var single = new ABPrintClassifiedText();
            try
            {
                single = await db.ABPrintClassifiedTexts.AsNoTracking().FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == id);
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
                totalCount = await db.ABPrintClassifiedTexts.AsNoTracking().CountAsync(d => d.PaymentModeId == paymentModeId);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return totalCount;
        }

        public async Task<ABPrintClassifiedText> GetDetails(int id, string bookingNo, int bookingStep = 0, int createdBy = 0)
        {
            var single = new ABPrintClassifiedText();
            try
            {
                single = await db.ABPrintClassifiedTexts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == id
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

        public async Task<View_ABPrintClassifiedText> GetDetailsView(int id, string bookingNo, int bookingStep = 0)
        {
            var single = new View_ABPrintClassifiedText();
            try
            {
                single = await db.View_ABPrintClassifiedTexts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == id
                        && d.BookingNo == bookingNo);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ABPrintClassifiedTextDetail>> GetABPrintClassifiedTextDetailListing(int abPrintClassifiedTextId)
        {
            var listing = new List<ABPrintClassifiedTextDetail>();

            try
            {
                IQueryable<ABPrintClassifiedTextDetail> query = db.ABPrintClassifiedTextDetails.Where(f => f.ABPrintClassifiedTextId == abPrintClassifiedTextId);
                listing = await query.AsNoTracking().ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<ABPrintClassifiedTextDetail>();
            }
        }

        public async Task<Response<ABPrintClassifiedText>> BookClassifiedTextAd(AdBookProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABPrintClassifiedText>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var newABPrintClassifiedText = model.ABPrintClassifiedText;

                    if (model.RequiredAdvertiser)
                    {
                        var newAdvertiser = model.Advertiser;

                        if (model.Advertiser.AdvertiserId > 0)
                        {
                            var updateAdvertiser = await db.Advertisers.FirstOrDefaultAsync(f => f.AdvertiserId == model.Advertiser.AdvertiserId);
                            if (updateAdvertiser != null)
                            {
                                updateAdvertiser.AdvertiserName = model.Advertiser.AdvertiserName;
                                updateAdvertiser.ModifiedBy = model.ABPrintClassifiedText.CreatedBy;
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
                        newABPrintClassifiedText.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    newABPrintClassifiedText.CreatedDate = currentDate;
                    //let's add ab print classified text
                    await db.ABPrintClassifiedTexts.AddAsync(newABPrintClassifiedText);
                    await db.SaveChangesAsync();

                    model.ABPrintClassifiedTextDetailListing.ForEach(f => f.ABPrintClassifiedTextId = newABPrintClassifiedText.ABPrintClassifiedTextId);

                    //let's add ab print classified text details
                    await db.ABPrintClassifiedTextDetails.AddRangeAsync(model.ABPrintClassifiedTextDetailListing);
                    await db.SaveChangesAsync();

                    response.Result = newABPrintClassifiedText;
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABPrintClassifiedText>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
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

        public async Task<Response<ABPrintClassifiedText>> EditBookedClassifiedTextAd(AdBookProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<ABPrintClassifiedText>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var classifiedText = model.ABPrintClassifiedText;

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
                                updateAdvertiser.ModifiedBy = model.ABPrintClassifiedText.CreatedBy;
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
                        classifiedText.AdvertiserId = newAdvertiser.AdvertiserId;
                    }

                    classifiedText.CreatedDate = currentDate;

                    var updateClassifiedText = await db.ABPrintClassifiedTexts.FirstOrDefaultAsync(f => f.ABPrintClassifiedTextId == classifiedText.ABPrintClassifiedTextId);

                    if (updateClassifiedText == null)
                        isOperationSuccess = false;

                    if (isOperationSuccess)
                    {
                        //let's update ab print classified text                                       
                        PopulateToUpdateClassifiedText(classifiedText, updateClassifiedText);
                        classifiedText.ModifiedDate = currentDate;

                        await db.SaveChangesAsync();

                        var existingDetailListing = await db.ABPrintClassifiedTextDetails
                                        .Where(f => f.ABPrintClassifiedTextId == classifiedText.ABPrintClassifiedTextId)
                                                .ToListAsync();

                        //reset ABPrintClassifiedTextDetails data
                        db.ABPrintClassifiedTextDetails.RemoveRange(existingDetailListing);
                        await db.SaveChangesAsync();

                        model.ABPrintClassifiedTextDetailListing.ForEach(f => f.ABPrintClassifiedTextId = classifiedText.ABPrintClassifiedTextId);

                        //let's add ab print classified text details
                        await db.ABPrintClassifiedTextDetails.AddRangeAsync(model.ABPrintClassifiedTextDetailListing);
                        await db.SaveChangesAsync();

                        response.Result = classifiedText;
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<ABPrintClassifiedText>() { IsSuccess = false, Message = "Error Ad Booking process. Please contact with support team!" };
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

        public async Task<Response<CheckoutProcessModel>> CheckoutClassifiedTextProcess(CheckoutProcessModel model)
        {
            bool isOperationSuccess = true;
            var response = new Response<CheckoutProcessModel>();
            var currentDate = DateTime.Now;

            using (var ts = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    //get classified display text
                    var classifiedText = await db.ABPrintClassifiedTexts.FirstOrDefaultAsync(f =>
                                                        f.ABPrintClassifiedTextId == model.ABPrintClassifiedText.ABPrintClassifiedTextId
                                                        && f.BookingNo == model.ABPrintClassifiedText.BookingNo);
                    if (classifiedText == null)
                    {
                        isOperationSuccess = false;
                        response = new Response<CheckoutProcessModel>() { IsSuccess = false, Message = "Error on Checkout process. Please contact with support team!" };
                    }

                    var updateClassifiedText = model.ABPrintClassifiedText;

                    classifiedText.AdStatus = updateClassifiedText.AdStatus;
                    classifiedText.GrossTotal = updateClassifiedText.GrossTotal;
                    classifiedText.DiscountPercent = updateClassifiedText.DiscountPercent;
                    classifiedText.NationalEdition = updateClassifiedText.NationalEdition;
                    classifiedText.OfferEditionId = updateClassifiedText.OfferEditionId;
                    classifiedText.DiscountAmount = updateClassifiedText.DiscountAmount;
                    classifiedText.NetAmount = updateClassifiedText.NetAmount;
                    classifiedText.VATAmount = updateClassifiedText.VATAmount;
                    classifiedText.Commission = updateClassifiedText.Commission;

                    classifiedText.ApprovalStatus = updateClassifiedText.ApprovalStatus;
                    classifiedText.PaymentModeId = updateClassifiedText.PaymentModeId;
                    classifiedText.PaymentStatus = updateClassifiedText.PaymentStatus;

                    classifiedText.BookingStep = updateClassifiedText.BookingStep;

                    classifiedText.BillNo = updateClassifiedText.BillNo;
                    classifiedText.BillDate = updateClassifiedText.BillDate;

                    classifiedText.ModifiedBy = updateClassifiedText.ModifiedBy;
                    classifiedText.ModifiedDate = currentDate;

                    await db.SaveChangesAsync();

                    if (isOperationSuccess)
                    {
                        var detailListing = await db.ABPrintClassifiedTextDetails
                                                .Where(f => f.ABPrintClassifiedTextId == model.ABPrintClassifiedTextId)
                                                .ToListAsync();

                        var newClassifiedTextDetailListing = new List<ABPrintClassifiedTextDetail>();
                        foreach (var existingDetail in detailListing)
                        {
                            foreach (var newDetail in model.ABPrintClassifiedTextDetailListing)
                            {
                                var newABPrintClassifiedTextDetail = new ABPrintClassifiedTextDetail
                                {
                                    ABPrintClassifiedTextId = existingDetail.ABPrintClassifiedTextId,
                                    AdContent = existingDetail.AdContent,
                                    EditionId = newDetail.EditionId,
                                    EditionPageId = newDetail.EditionPageId,
                                    PublishDate = existingDetail.PublishDate
                                };

                                newClassifiedTextDetailListing.Add(newABPrintClassifiedTextDetail);
                            }
                        }

                        //reset ABPrintClassifiedTextDetails data
                        db.ABPrintClassifiedTextDetails.RemoveRange(detailListing);
                        await db.SaveChangesAsync();

                        //let's add ab print classified text details
                        await db.ABPrintClassifiedTextDetails.AddRangeAsync(newClassifiedTextDetailListing);
                        await db.SaveChangesAsync();

                        response.Result = model;
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    response = new Response<CheckoutProcessModel>() { IsSuccess = false, Message = "Error Checkout process. Please contact with support team!" };
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

        public async Task<bool> Update(ABPrintClassifiedText abPrintClassifiedText)
        {
            try
            {
                var upateABPrintClassifiedText = await db.ABPrintClassifiedTexts.FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == abPrintClassifiedText.ABPrintClassifiedTextId);

                if (upateABPrintClassifiedText == null)
                    return false;

                upateABPrintClassifiedText.CategoryId = abPrintClassifiedText.CategoryId;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(ABPrintClassifiedText abPrintClassifiedText)
        {
            try
            {
                var removeABPrintClassifiedText = await db.ABPrintClassifiedTexts.FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == abPrintClassifiedText.ABPrintClassifiedTextId);

                if (removeABPrintClassifiedText == null)
                    return false;

                db.ABPrintClassifiedTexts.Remove(removeABPrintClassifiedText);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentInfo(ABPrintClassifiedText classifiedText)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateClassifiedText = await db.ABPrintClassifiedTexts.FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == classifiedText.ABPrintClassifiedTextId);

                if (updateClassifiedText == null)
                    return false;

                updateClassifiedText.PaymentType = classifiedText.PaymentType;
                updateClassifiedText.PaymentMethod = classifiedText.PaymentMethod;
                updateClassifiedText.PaymentMobileNumber = classifiedText.PaymentMobileNumber;
                updateClassifiedText.PaymentTrxId = classifiedText.PaymentTrxId;
                updateClassifiedText.PaymentPaidAmount = classifiedText.PaymentPaidAmount;
                updateClassifiedText.BookingStep = classifiedText.BookingStep;

                updateClassifiedText.ModifiedBy = classifiedText.ModifiedBy;
                updateClassifiedText.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UpdatePaymentMode(ABPrintClassifiedText classifiedText)
        {
            var currentDate = DateTime.Now;

            try
            {
                var updateABPrintClassifiedText = await db.ABPrintClassifiedTexts.FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == classifiedText.ABPrintClassifiedTextId);

                if (updateABPrintClassifiedText == null)
                    return false;

                updateABPrintClassifiedText.ApprovalStatus = classifiedText.ApprovalStatus;
                updateABPrintClassifiedText.PaymentStatus = classifiedText.PaymentStatus;
                updateABPrintClassifiedText.AdStatus = classifiedText.AdStatus;

                if (classifiedText.ActualReceived > 0)
                    updateABPrintClassifiedText.ActualReceived = classifiedText.ActualReceived;

                if (!string.IsNullOrWhiteSpace(classifiedText.PaymentTrxId))
                    updateABPrintClassifiedText.PaymentTrxId = classifiedText.PaymentTrxId;

                if (!string.IsNullOrWhiteSpace(classifiedText.MoneyReceiptNo))
                    updateABPrintClassifiedText.MoneyReceiptNo = classifiedText.MoneyReceiptNo;

                if (!string.IsNullOrWhiteSpace(classifiedText.PaymentMobileNumber))
                    updateABPrintClassifiedText.PaymentMobileNumber = classifiedText.PaymentMobileNumber;

                if (classifiedText.PaymentModeId > 0)
                    updateABPrintClassifiedText.PaymentModeId = classifiedText.PaymentModeId;

                updateABPrintClassifiedText.ModifiedBy = classifiedText.ModifiedBy;
                updateABPrintClassifiedText.ModifiedDate = currentDate;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<string> CheckAgency(int id,string bookingno)
        {
            try
            {
                var upateABPrintClassifiedText = await db.ABPrintClassifiedTexts.FirstOrDefaultAsync(d => d.ABPrintClassifiedTextId == id && d.BookingNo==bookingno);

                if (upateABPrintClassifiedText.AgencyId == null || upateABPrintClassifiedText.AgencyId == 0)
                {
                    return "F";
                }
                else
                {
                    return "T";
                }

               
            }
            catch (Exception ex)
            {
                return "F";
            }
        }

        #endregion

        #region Private Methods
        private void PopulateToUpdateClassifiedText(ABPrintClassifiedText classifiedText, ABPrintClassifiedText updateClassifiedText)
        {
            updateClassifiedText.CategoryId = classifiedText.CategoryId;
            updateClassifiedText.SubCategoryId = classifiedText.SubCategoryId;
            updateClassifiedText.TotalCount = classifiedText.TotalCount;
            updateClassifiedText.IsBigBulletPointSingle = classifiedText.IsBigBulletPointSingle;
            updateClassifiedText.IsBigBulletPointDouble = classifiedText.IsBigBulletPointDouble;
            updateClassifiedText.IsBold = classifiedText.IsBold;
            updateClassifiedText.IsBoldinScreen = classifiedText.IsBoldinScreen;
            updateClassifiedText.IsBoldScreenSingleBox = classifiedText.IsBoldScreenSingleBox;
            updateClassifiedText.IsBoldScreenDoubleBox = classifiedText.IsBoldScreenDoubleBox;
            updateClassifiedText.AdvertiserId = classifiedText.AdvertiserId;
            updateClassifiedText.GrossTotal = classifiedText.GrossTotal;
            updateClassifiedText.DiscountPercent = classifiedText.DiscountPercent;
            updateClassifiedText.DiscountAmount = classifiedText.DiscountAmount;
            updateClassifiedText.NetAmount = classifiedText.NetAmount;
            updateClassifiedText.VATAmount = classifiedText.VATAmount;
            updateClassifiedText.Commission = classifiedText.Commission;
            updateClassifiedText.OfferDateId = classifiedText.OfferDateId;
            updateClassifiedText.OfferEditionId = classifiedText.OfferEditionId;
            updateClassifiedText.UpazillaId = classifiedText.UpazillaId;
            updateClassifiedText.AgencyId = classifiedText.AgencyId;
            updateClassifiedText.BrandId = classifiedText.BrandId;
            updateClassifiedText.BookingStep = classifiedText.BookingStep;
            updateClassifiedText.ModifiedBy = classifiedText.ModifiedBy;
            updateClassifiedText.PaymentModeId = classifiedText.PaymentModeId;
            updateClassifiedText.ApprovalStatus = classifiedText.ApprovalStatus;
            updateClassifiedText.PaymentStatus = classifiedText.PaymentStatus;
            updateClassifiedText.AdStatus = classifiedText.AdStatus;
            updateClassifiedText.Remarks = classifiedText.Remarks;
        }
        #endregion
    }
}
