

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbAd.Data.Models
{
    [Table("ABPrintClassifiedDisplay")]
    public class ABPrintClassifiedDisplay
    {
        [Key]
        public int ABPrintClassifiedDisplayId { get; set; }

        [Display(Name = "Booking No")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string BookingNo { get; set; }

        [Display(Name = "Category Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CategoryId { get; set; }

        [Display(Name = "Sub Category Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Total Title Word Count")]
        [Required(ErrorMessage = "{0} is Required")]
        public int TotalTitleWordCount { get; set; }

        [Display(Name = "Total Ad Content Word Count")]
        [Required(ErrorMessage = "{0} is Required")]
        public int TotalAdContentWordCount { get; set; }

        [Display(Name = "Booked By")]
        [Required(ErrorMessage = "{0} is Required")]
        public int BookedBy { get; set; }

        [Display(Name = "Book Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime BookDate { get; set; }

        [Display(Name = "Advertiser Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int AdvertiserId { get; set; }

        [Display(Name = "Actual Received")]
        public decimal? ActualReceived { get; set; }

        [Display(Name = "Gross Total")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal GrossTotal { get; set; }

        [Display(Name = "Discount Percent")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "Discount Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Net Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal NetAmount { get; set; }

        [Display(Name = "V A T Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal VATAmount { get; set; }

        [Display(Name = "Commission")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal Commission { get; set; }

        [Display(Name = "Offer Date Id")]
        public int? OfferDateId { get; set; }

        [Display(Name = "Offer Edition Id")]
        public int? OfferEditionId { get; set; }

        [Display(Name = "Upazilla Id")]
        public int? UpazillaId { get; set; }

        [Display(Name = "Agency Id")]
        public int? AgencyId { get; set; }

        [Display(Name = "Brand Id")]
        public int? BrandId { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CreatedBy { get; set; }

        [Display(Name = "Booking Step")]
        [Required(ErrorMessage = "{0} is Required")]
        public int BookingStep { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public int? ModifiedBy { get; set; }

        [Display(Name = "Payment Mode Id")]
        public int? PaymentModeId { get; set; }

        [Display(Name = "Approval Status")]
        public int? ApprovalStatus { get; set; }

        [Display(Name = "Payment Status")]
        public int? PaymentStatus { get; set; }

        [Display(Name = "Ad Status")]
        [Required(ErrorMessage = "{0} is Required")]
        public int AdStatus { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Remarks { get; set; }

        [Display(Name = "Money Receipt No")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string MoneyReceiptNo { get; set; }

        [Display(Name = "Ad Column Inch")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal AdColumnInch { get; set; }

        [Display(Name = "Complete Matter")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool CompleteMatter { get; set; }

        [Display(Name = "All Edition")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool NationalEdition { get; set; }

        [Display(Name = "Payment Type")]
        public int? PaymentType { get; set; }

        [Display(Name = "Payment Method")]
        public int? PaymentMethod { get; set; }

        [Display(Name = "Payment Mobile Number")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string PaymentMobileNumber { get; set; }

        [Display(Name = "Payment Trx Id")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string PaymentTrxId { get; set; }

        [Display(Name = "Payment Paid Amount")]
        public decimal? PaymentPaidAmount { get; set; }

        [Display(Name = "Bill U R L")]
        [StringLength(250, ErrorMessage = "Maximum length is {1}")]
        public string BillURL { get; set; }

        [Display(Name = "Pay Channel Id")]
        public int? PayChannelId { get; set; }

        [Display(Name = "Bill Date")]
        public DateTime? BillDate { get; set; }

        [Display(Name = "Bill No")]
        [StringLength(16, ErrorMessage = "Maximum length is {1}")]
        public string BillNo { get; set; }

        [Display(Name = "Cancel Reason Id")]
        public int? CancelReasonId { get; set; }

        [Display(Name = "Cancel Reason Detail")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string CancelReasonDetail { get; set; }
        public bool IsCorrespondent { get; set; } 
        public int? CollectorId { get; set; }

        //aditional
        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public string AdContent { get; set; }

    }
}