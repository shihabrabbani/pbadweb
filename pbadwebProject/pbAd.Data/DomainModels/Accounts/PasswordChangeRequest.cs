using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.Accounts
{
    public class PasswordChangeRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public bool IsChanged { get; set; }
        public string Confirmation { get; set; }
        public bool ValidateOldPassword { get; set; }
        public bool GenerateNewPassword { get; set; }
        public string PasswordResetToken { get; set; }
    }

    public class OTPChangeRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public bool IsChanged { get; set; }
        public string Confirmation { get; set; }

        public string OTP { get; set; }
        public DateTime OTPSentTime { get; set; }
        public DateTime OTPExpireTime { get; set; }
    }
}
