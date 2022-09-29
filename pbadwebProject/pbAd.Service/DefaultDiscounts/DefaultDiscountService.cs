#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.DefaultDiscounts
{
    public class DefaultDiscountService: IDefaultDiscountService
    {
        #region Private Members
        private readonly pbAdContext db; 
        #endregion

        #region Ctor
        public DefaultDiscountService(pbAdContext db)
        {
            this.db = db;
        } 
        #endregion

        #region Public Methods
       
        public async Task<DefaultDiscount> GetDetailsById(int id)
        {
            var single = new DefaultDiscount();
            try
            {
                single = await db.DefaultDiscounts.FirstOrDefaultAsync(d => d.DefaultDiscountId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<DefaultDiscount> GetDetailsByAdTypeAndPaymentMode(string adType , int paymentModeId)
        {
            var single = new DefaultDiscount();
            try
            {
                single = await db.DefaultDiscounts
                    .FirstOrDefaultAsync(d => d.AdType == adType
                                        && d.PaymentModeId==paymentModeId);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<IEnumerable<ConstantDropdownItem>> GetDefaultDiscountListForDropdown()
        {
            IQueryable<DefaultDiscount> query = db.DefaultDiscounts;

            var itemList = await query.ToListAsync();

            return itemList.Select(f => new ConstantDropdownItem
            {
                Text = f.DiscountRate.ToString(),
                Value = f.DefaultDiscountId.ToString()
            });
        }


        public async Task<IEnumerable<DefaultDiscount>> GetAllDefaultDiscounts()
        {
            var listing = new List<DefaultDiscount>();

            try
            {
                IQueryable<DefaultDiscount> query = db.DefaultDiscounts;
                listing = await query.ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return null;
            }           
        }

        public async Task<bool> Add(DefaultDiscount brand)
        {
            try
            {
                await db.DefaultDiscounts.AddAsync(brand);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(DefaultDiscount brand)
        {
            try
            {
                var upateDefaultDiscount = await db.DefaultDiscounts.FirstOrDefaultAsync(d => d.DefaultDiscountId == brand.DefaultDiscountId);

                if (upateDefaultDiscount == null)
                    return false;
                
                upateDefaultDiscount.DiscountRate = brand.DiscountRate;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(DefaultDiscount brand)
        {
            try
            {
                var removeDefaultDiscount = await db.DefaultDiscounts.FirstOrDefaultAsync(d => d.DefaultDiscountId == brand.DefaultDiscountId);

                if (removeDefaultDiscount == null)
                    return false;

                db.DefaultDiscounts.Remove(removeDefaultDiscount);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
