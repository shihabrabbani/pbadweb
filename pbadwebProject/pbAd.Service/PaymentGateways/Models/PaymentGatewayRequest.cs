using System;
using System.Collections.Generic;
using System.Text;

namespace pbAd.Service.PaymentGateways.Models
{
    public class PaymentGatewayRequest
    {
        public decimal total_amount { get; set; }
        public string tran_id { get; set; }
        public string success_url { get; set; }
        public string fail_url { get; set; }
        public string cancel_url { get; set; }
        public string version { get; set; }
        public string cus_name { get; set; }
        public string cus_email { get; set; }
        public string cus_add1 { get; set; }
        public string cus_add2 { get; set; }
        public string cus_city { get; set; }
        public string cus_state { get; set; }
        public string cus_postcode { get; set; }
        public string cus_country { get; set; }
        public string cus_phone { get; set; }
        public string cus_fax { get; set; }
        public string ship_name { get; set; }
        public string ship_add1 { get; set; }
        public string ship_add2 { get; set; }
        public string ship_city { get; set; }
        public string ship_state { get; set; }
        public string ship_postcode { get; set; }
        public string ship_country { get; set; }
        public string value_a { get; set; }
        public string value_b { get; set; }
        public string value_c { get; set; }
        public string value_d { get; set; }
        public string shipping_method { get; set; }
        public int num_of_item { get; set; }
        public string product_name { get; set; }
        public string product_profile { get; set; }
        public string product_category { get; set; }
        public bool IsSandbox { get; set; }
    }
}
