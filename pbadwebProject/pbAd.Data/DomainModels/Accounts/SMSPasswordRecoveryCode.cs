using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Data.DomainModels.Accounts
{
    public class SMSPasswordRecoveryCode : SmsMessageBase
    {
        public string PasswordResetToken { get; set; }
    }

    public class SMSWelcomeMessage : SmsMessageBase
    {
        public string FullName { get; set; }
    }
    public class SMSOrderConfirmationMessage : SmsMessageBase
    {
        public string FullName { get; set; }
        public string NetAmount { get; set; }
        public string OrderNo { get; set; }
        public string TotalProduct { get; set; }
    }

    public class SmsMessageBase
    {
        public string ToNumber { get; set; }
        public string FromNumber { get; set; }

        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
    }
}
