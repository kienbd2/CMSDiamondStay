﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class DashboardAdmin
    {
        public int total_user { get; set; }
        public double total { get; set; }
        public double total_month { get; set; }
        public int total_apartment { get; set; }
        public int total_booking { get; set; }
        public int total_booking_finish { get; set; }
    }
}