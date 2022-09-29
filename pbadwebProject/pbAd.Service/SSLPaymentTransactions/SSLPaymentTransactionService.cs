#region Usings
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.SSLPaymentTransactions
{
    public class SSLPaymentTransactionService : ISSLPaymentTransactionService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public SSLPaymentTransactionService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods

        public async Task<SSLPaymentTransaction> GetDetailsById(int id)
        {
            var single = new SSLPaymentTransaction();
            try
            {
                single = await db.SSLPaymentTransactions.FirstOrDefaultAsync(d => d.AutoId == id);
            }
            catch (Exception ex)
            {
                return null;
            }
            return single;
        }

        public async Task<bool> Add(SSLPaymentTransaction sslPaymentTransaction)
        {
            var currentDate = DateTime.Now;

            try
            {
                sslPaymentTransaction.CreatedDate = currentDate;
                await db.SSLPaymentTransactions.AddAsync(sslPaymentTransaction);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(SSLPaymentTransaction sslPaymentTransaction)
        {
            var currentDate = DateTime.Now;
            try
            {
                var upateSSLPaymentTransaction = await db.SSLPaymentTransactions
                    .FirstOrDefaultAsync(d => d.AdMasterId == sslPaymentTransaction.AdMasterId &&
                    d.AdType == sslPaymentTransaction.AdType
                    );

                if (upateSSLPaymentTransaction == null)
                    return false;

                upateSSLPaymentTransaction.Tran_Id = sslPaymentTransaction.Tran_Id;
                upateSSLPaymentTransaction.Val_Id = sslPaymentTransaction.Val_Id;
                upateSSLPaymentTransaction.Amount = sslPaymentTransaction.Amount;
                upateSSLPaymentTransaction.Cart_Type = sslPaymentTransaction.Cart_Type;
                upateSSLPaymentTransaction.Store_Amount = sslPaymentTransaction.Store_Amount;
                upateSSLPaymentTransaction.Card_No = sslPaymentTransaction.Card_No;
                upateSSLPaymentTransaction.Bank_Tran_Id = sslPaymentTransaction.Bank_Tran_Id;
                upateSSLPaymentTransaction.Status = sslPaymentTransaction.Status;
                upateSSLPaymentTransaction.Tran_Date = sslPaymentTransaction.Tran_Date;
                upateSSLPaymentTransaction.Error = sslPaymentTransaction.Error;
                upateSSLPaymentTransaction.Currency = sslPaymentTransaction.Currency;
                upateSSLPaymentTransaction.Card_Issuer = sslPaymentTransaction.Card_Issuer;
                upateSSLPaymentTransaction.Card_Brand = sslPaymentTransaction.Card_Brand;
                upateSSLPaymentTransaction.Card_Sub_Brand = sslPaymentTransaction.Card_Sub_Brand;
                upateSSLPaymentTransaction.Card_Issuer_Country = sslPaymentTransaction.Card_Issuer_Country;
                upateSSLPaymentTransaction.Card_Issuer_Country_Code = sslPaymentTransaction.Card_Issuer_Country_Code;
                upateSSLPaymentTransaction.Store_Id = sslPaymentTransaction.Store_Id;
                upateSSLPaymentTransaction.Verify_Sign = sslPaymentTransaction.Verify_Sign;
                upateSSLPaymentTransaction.Verify_Key = sslPaymentTransaction.Verify_Key;
                upateSSLPaymentTransaction.Verify_Sign_Sha2 = sslPaymentTransaction.Verify_Sign_Sha2;
                upateSSLPaymentTransaction.Currency_Type = sslPaymentTransaction.Currency_Type;
                upateSSLPaymentTransaction.Currency_Amount = sslPaymentTransaction.Currency_Amount;
                upateSSLPaymentTransaction.Currency_Rate = sslPaymentTransaction.Currency_Rate;
                upateSSLPaymentTransaction.Base_Fair = sslPaymentTransaction.Base_Fair;
                upateSSLPaymentTransaction.Value_A = sslPaymentTransaction.Value_A;
                upateSSLPaymentTransaction.Value_B = sslPaymentTransaction.Value_B;
                upateSSLPaymentTransaction.Value_C = sslPaymentTransaction.Value_C;
                upateSSLPaymentTransaction.Value_D = sslPaymentTransaction.Value_D;
                upateSSLPaymentTransaction.Risk_Level = sslPaymentTransaction.Risk_Level;
                upateSSLPaymentTransaction.Risk_Title = sslPaymentTransaction.Risk_Title;

                if (!string.IsNullOrWhiteSpace(sslPaymentTransaction.FailedReason))
                    upateSSLPaymentTransaction.FailedReason = sslPaymentTransaction.FailedReason;

                upateSSLPaymentTransaction.PaymentTransactionStatus = sslPaymentTransaction.PaymentTransactionStatus;
                upateSSLPaymentTransaction.UpdatedDate = currentDate;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(SSLPaymentTransaction sslPaymentTransaction)
        {
            try
            {
                var removeSSLPaymentTransaction = await db.SSLPaymentTransactions.FirstOrDefaultAsync(d => d.AdMasterId == sslPaymentTransaction.AdMasterId);

                if (removeSSLPaymentTransaction == null)
                    return false;

                db.SSLPaymentTransactions.Remove(removeSSLPaymentTransaction);
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
