using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CMSDiamondStay.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMSDiamondStay.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["Authent"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");

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
        public ActionResult Create(Apartments apartments)
        {
            var lstImage = new List<string>();
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                Account account = new Account("dev2020", "247996535991499", "9jI_5YjJaseBKUrY929sUtt0Fy0");

                string path = Path.Combine(Server.MapPath("Images"), Path.GetFileName(file.FileName));

                Cloudinary cloudinary = new Cloudinary(account);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path, file.InputStream),
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                lstImage.Add(uploadResult.SecureUrl.ToString());

            }
            var apartment = new ApartmentsViewModel();
            apartment.name = apartments.name; //Reading text box values using Jquery
            apartment.amount_bed = apartments.amount_bed;
            apartment.amount_sofa = apartments.amount_sofa;
            apartment.capacity_standard = apartments.capacity_standard;
            apartment.capacity_max = apartments.capacity_max;
            apartment.relax_suggest = apartments.relax_suggest;
            apartment.direction_instruction = apartments.direction_instruction;
            apartment.cuisine_suggest = apartments.cuisine_suggest;
            apartment.regulation = apartments.regulation;
            apartment.phone = apartments.phone;
            apartment.fax = apartments.fax;
            apartment.area = apartments.area;
            apartment.amount_bathroom = apartments.amount_bathroom;
            apartment.amount_bedroom = apartments.amount_bedroom;
            apartment.star_standard = apartments.star_standard;
            apartments.conveniences.RemoveAt(apartments.conveniences.Count - 1);
            apartment.conveniences = apartments.conveniences;
            apartment.cancel_policy = apartments.cancel_policy;
            apartment.latitude = apartments.latitude;
            apartment.longitude = apartments.longitude;
            string[] arrListStr = apartments.travelfrom.Split(',');
            apartment.detail_address = arrListStr[0];
            apartment.village_address = arrListStr[1];
            apartment.district_address = arrListStr[2];
            apartment.province_address = arrListStr[3];
            apartment.apartment_type = apartments.apartment_type;
            apartment.price = apartments.price;
            apartment.price_promotion = apartments.price_promotion;
            apartment.min_day = apartments.min_day;
            apartment.max_day = apartments.max_day;
            apartment.surcharge_per_person = apartments.surcharge_per_person;
            apartment.check_in_time = apartments.check_in_time;
            apartment.check_out_time = apartments.check_out_time;
            apartment.description = apartments.description;
            apartment.gallery = lstImage;

            if (Session["Authent"] != null)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://35.197.153.19:12345/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                    //HTTP POST
                    Task task = Task.Run(async () =>
                    {
                        var response = await client.PostAsync("manager/apartments", new StringContent(
       new JavaScriptSerializer().Serialize(apartment), Encoding.UTF8, "application/json"));
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var EmpResponse = await response.Content.ReadAsStringAsync();

                    //var result = postTask.Result;
                    if (response.IsSuccessStatusCode)
                    {
                            TempData["message"] = "Create Success";
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                            TempData["message"] = serializer.Deserialize<dynamic>(EmpResponse)["message"];
                        return RedirectToAction("CreateApartment", "Home");
                    }
                    });
                    task.Wait();

                }
            }

            
            return View();


        }
    }

}