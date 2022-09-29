using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Service.EmailSenders
{
    public class EmailSenderModel
    {
        public string ToEmail { get; set; }
        public string OTP { get; set; }

        public string FromEmail { get; set; }
        public string FromEmailName { get; set; }

        public string SMTPMailServer { get; set; }
        public int SMTPMailServerPort { get; set; }

        public string MailNetworkUsername { get; set; }
        public string MailNetworkPassword { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
