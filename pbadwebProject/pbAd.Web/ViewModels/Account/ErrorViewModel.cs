using System.ComponentModel.DataAnnotations;

namespace pbAd.Web.ViewModels.Account
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    } 
}
