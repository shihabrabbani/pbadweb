using pbAd.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Checkout
{
    public class PaymentViewModel
    {
        public int MasterId { get; set; }
        public string BookingNo { get; set; }

        [Display(Name = "Payment Type")]
        [Required(ErrorMessage = "{0} is Required")]
        public int PaymentType { get; set; }

        [Display(Name = "Payment Method")]        
        public int PaymentMethod { get; set; }

        //cash/card

        [Display(Name = "Mobile Number")]
        [StringLength(15, ErrorMessage = "Maximum length is {1}")]        
        public string PaymentMobileNumber { get; set; }

        [Display(Name = "Trx Id")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]        
        public string PaymentTrxId { get; set; }

        [Display(Name = "Paid Amount")]       
        public decimal? PaymentPaidAmount { get; set; }

        //check/payout
        [Display(Name = "Check No and Check Date")]        
        public string CheckInfo { get; set; }

        [Display(Name = "Bank and Branch Name")]       
        public string BankInfo { get; set; }

        [Display(Name = "Amount")]
        public decimal? CheckOrPayorderAmount { get; set; }

        [Display(Name = "Net Payable")]
        public int NetPayable { get; set; }
        public int PaymentModeId { get; set; }

        public string CardPaymentUrl { get; set; }
    }
}
