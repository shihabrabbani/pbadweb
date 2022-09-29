using pbAd.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.AdBookingReports
{
    public class AdBookingUploadLaterViewModel
    {
        public int AutoId { get; set; }
        public string AdType { get; set; }
        public string BookingNumber { get; set; }
        public string BookedBy { get; set; }
        public DateTime BookingDate { get; set; }
        public int NetPayable { get; set; }

        [Display(Name = "Publish Date")]
        [Required(ErrorMessage ="Publish Date is Required")]       
        public DateTime? BillDate { get; set; }

        [Display(Name = "Upload Image")]
        [Required(ErrorMessage = "Please upload file")]
        public List<IFormFile> ImageContents { get; set; }

        public string RemoveUploadedFiles { get; set; }
        public string RemoveExistingFiles { get; set; }

        public IEnumerable<PrivateDisplayMediaContent> MediaContents { get; set; }
    }
}
