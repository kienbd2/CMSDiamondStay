using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMSDiamondStay.Models
{
    public class ConvenienceViewModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Tên tiện ích")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Mô tả")]
        public string description { get; set; }
    }
}