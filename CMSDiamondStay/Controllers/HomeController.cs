using CMSDiamondStay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMSDiamondStay.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

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
            if (Session["Authent"] != null)
            {
                ViewBag.convenience = getAllConvenience();
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public JsonResult Create( Apartments apartments)
        {
            if (ModelState.IsValid)
            {
                if (Session["Authent"] != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://35.197.153.19:12345/");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                        //HTTP POST
                        var response = client.PostAsync("manager/apartments", new StringContent(
           new JavaScriptSerializer().Serialize(apartments), Encoding.UTF8, "application/json"));


                        //var result = postTask.Result;
                        if (response.Result.IsSuccessStatusCode)
                        {
                            TempData["message"] = "Create Success";
                        }

                    }
                }

                ModelState.AddModelError("", "Identifiant ou mot de passe invalide");
                return Json("error-model-wrong");
            }
            return Json("error-mode-not-valid");
        }
    }

}