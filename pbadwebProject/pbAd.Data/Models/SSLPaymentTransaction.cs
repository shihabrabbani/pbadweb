using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("SSLPaymentTransaction")]
    public class SSLPaymentTransaction
    {
        [Key]

        public int AutoId { get; set; }

        [Display(Name = "Ad Type")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string AdType { get; set; }

        [Display(Name = "Ad Master Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int AdMasterId { get; set; }

        [Display(Name = "Tran Id")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Tran_Id { get; set; }

        [Display(Name = "Val Id")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Val_Id { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Cart Type")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Cart_Type { get; set; }

        [Display(Name = "Store Amount")]
        public decimal Store_Amount { get; set; }

        [Display(Name = "Card No")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Card_No { get; set; }

        [Display(Name = "Bank Tran Id")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Bank_Tran_Id { get; set; }

        [Display(Name = "Status")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Status { get; set; }

        [Display(Name = "Tran Date")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Tran_Date { get; set; }

        [Display(Name = "Error")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Error { get; set; }

        [Display(Name = "Currency")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Currency { get; set; }

        [Display(Name = "Card Issuer")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Card_Issuer { get; set; }

        [Display(Name = "Card Brand")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Card_Brand { get; set; }

        [Display(Name = "Card Sub Brand")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Card_Sub_Brand { get; set; }

        [Display(Name = "Card Issuer Country")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Card_Issuer_Country { get; set; }

        [Display(Name = "Card Issuer Country Code")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Card_Issuer_Country_Code { get; set; }

        [Display(Name = "Store Id")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Store_Id { get; set; }

        [Display(Name = "Verify Sign")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Verify_Sign { get; set; }

        [Display(Name = "Verify Key")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Verify_Key { get; set; }

        [Display(Name = "Verify Sign Sha2")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Verify_Sign_Sha2 { get; set; }

        [Display(Name = "Currency Type")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Currency_Type { get; set; }

        [Display(Name = "Currency Amount")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Currency_Amount { get; set; }

        [Display(Name = "Currency Rate")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Currency_Rate { get; set; }

        [Display(Name = "Base Fair")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Base_Fair { get; set; }

        [Display(Name = "Value A")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Value_A { get; set; }

        [Display(Name = "Value B")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Value_B { get; set; }

        [Display(Name = "Value C")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Value_C { get; set; }

        [Display(Name = "Value D")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Value_D { get; set; }

        [Display(Name = "Risk Level")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Risk_Level { get; set; }

        [Display(Name = "Risk Title")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Risk_Title { get; set; }

        [Display(Name = "Gatway Note")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string GatwayNote { get; set; }

        [Display(Name = "Failed Reason")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string FailedReason { get; set; }

        [Display(Name = "Redirect Gateway U R L")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string RedirectGatewayURL { get; set; }

        [Display(Name = "Redirect Gateway U R L Failed")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string RedirectGatewayURLFailed { get; set; }

        [Display(Name = "Payment Transaction Status")]
        [Required(ErrorMessage = "{0} is Required")]
        public int PaymentTransactionStatus { get; set; }

        [Display(Name = "Created By")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated By")]
        public int? UpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

    }
}