using System;

namespace pbAd.Web.ViewModels.Account
{
    public class AdvertiserViewModel
    {
        public int AdvertiserId { get; set; }        
        public string AdvertiserGuidId { get; set; }       
        public string AdvertiserBarcode { get; set; }
        public string AdvertiserName { get; set; }
        public string AdvertiserEmail { get; set; }
        public string AdvertiserMobileNo { get; set; }
        public int VerificationStatus { get; set; }
        public int? RegistrationType { get; set; }
        public string AdvertiserPassword { get; set; }
        public int? LocationId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
