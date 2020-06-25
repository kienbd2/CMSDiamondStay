using CMSDiamondStay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSDiamondStay.Controllers
{
    public class BaseController : Controller
    {
        public string Authent { get; set; }
    }
}