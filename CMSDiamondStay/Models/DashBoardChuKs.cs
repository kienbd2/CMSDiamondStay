using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class DashBoardChuKs
    {
        public int total_good_review { get; set; }
        public int total_review { get; set; }
        public int total_apartment_rented { get; set; }
        public decimal total { get; set; }
        public decimal total_month { get; set; }
        public int total_apartment { get; set; }
        public int total_booking { get; set; }
        public int total_booking_finish { get; set; }
    }
}