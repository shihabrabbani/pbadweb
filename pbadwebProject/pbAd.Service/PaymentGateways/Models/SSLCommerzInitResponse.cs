using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Service.PaymentGateways.Models
{
    public class SSLCommerzInitResponse
    {
        public string status { get; set; }
        public string failedreason { get; set; }
        public string sessionkey { get; set; }
        public Gw gw { get; set; }
        public string redirectGatewayURL { get; set; }
        public string redirectGatewayURLFailed { get; set; }
        public string GatewayPageURL { get; set; }
        public string storeBanner { get; set; }
        public string storeLogo { get; set; }
        public List<Desc> desc { get; set; }
        public string is_direct_pay_enable { get; set; }
    }

    public class Gw
    {
        public string visa { get; set; }
        public string master { get; set; }
        public string amex { get; set; }
        public string othercards { get; set; }
        public string internetbanking { get; set; }
        public string mobilebanking { get; set; }
    }

    public class Desc
    {
        public string name { get; set; }
        public string type { get; set; }
        public string logo { get; set; }
        public string gw { get; set; }
    }
}
