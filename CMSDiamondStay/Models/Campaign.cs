using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class Campaign
    {
        public int amount { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int percent { get; set; }
        public string code { get; set; }
    }
}