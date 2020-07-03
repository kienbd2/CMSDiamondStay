using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class User
    {
        public string user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public bool is_activated { get; set; }
        public DateTime modifiedTime { get; set; }
        public bool first_time { get; set; }
        public int role { get; set; }
        public string password { get; set; }
        public int main_balance { get; set; }
    }
}