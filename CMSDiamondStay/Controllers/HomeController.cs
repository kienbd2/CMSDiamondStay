using CMSDiamondStay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSDiamondStay.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["Authent"] != null)
            {
                var a = Session["Authent"] as string;
                var d = a.Split('.');
                var b = "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJkaWFtb25kc3RheWNvbXBhbnlAZ21haWwuY29tIiwiZXhwIjoxNTkzOTQ0Mjk2fQ.waKvN5OTBSXitYO-aNu_8e9nT-Fs27X_vp8WiNBKFn8n7KUt8W6Sx2TAyfToJhIl2zlv3lK0SrD8r0QENcvcyg";
              if (a == b)
                {
                    var c = a;
                }
            
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CreateApartment()
        {
            return View();
        }
    }
}