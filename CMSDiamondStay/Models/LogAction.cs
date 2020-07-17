using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class LogAction
    {
        public int id { get; set; }
        public string action { get; set; }
        public string table_access { get; set; }
        public DateTime action_time { get; set; }
        public string ip { get; set; }
    }
}