using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class Booking
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string code { get; set; }
        public string apartment_id { get; set; }
        public string apartment_name { get; set; }
        public string apartment_thumb { get; set; }
        public string apartment_type { get; set; }
        public string check_in { get; set; }
        public string check_out { get; set; }
        public int status { get; set; }
        public string amount { get; set; }
    }
}