using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class Apartments
    {
        public string name { get; set; }
        public int amount_bed { get; set; }
        public int amount_sofa { get; set; }
        public int capacity_standard { get; set; }
        public int capacity_max { get; set; }
        public string relax_suggest { get; set; }
        public string direction_instruction { get; set; }
        public string cuisine_suggest { get; set; }
        public string regulation { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public float area { get; set; }
        public int amount_bathroom { get; set; }
         public int amount_bedroom { get; set; }
        public int star_standard { get; set; }
        public List<int> conveniences { get; set; }
        public int cancel_policy { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }

        public string detail_address { get; set; }
        public string village_address { get; set; }
        public string district_address { get; set; }
        public string province_address { get; set; }
        public int apartment_type { get; set; }

        public List<string> gallery { get; set; }
        public float price { get; set; }
        public float price_promotion { get; set; }
        public int min_day { get; set; }
        public int max_day { get; set; }
        public float surcharge_per_person { get; set; }
        public string check_in_time { get; set; }

        public string check_out_time { get; set; }
    }
}