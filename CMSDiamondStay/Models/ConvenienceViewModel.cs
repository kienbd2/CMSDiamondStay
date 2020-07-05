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
        [Display(Name = "Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string description { get; set; }
    }
}