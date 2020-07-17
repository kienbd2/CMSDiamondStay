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
        string Baseurl = "http://35.240.224.17:12345";
        public ActionResult Index()
        {
            if (Session["Authent"] != null)
            {
                DashboardAdmin dtoAdmin = new DashboardAdmin();
                DashBoardChuKs dtoChuKs = new DashBoardChuKs();

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    Task task = Task.Run(async () =>
                    {
                        HttpResponseMessage Res = await client.GetAsync($"/manager/dashboard/info");

                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"];
                            int role = Convert.ToInt32(Session["role"]);
                            if (role == 2 || role==3|| role==4|| role==5)
                            {
                                dtoAdmin.total_user = jsonObject["total_user"];
                                dtoAdmin.total = Convert.ToDouble(jsonObject["total"]);
                                dtoAdmin.total_month = Convert.ToDouble(jsonObject["total_month"]);
                                dtoAdmin.total_apartment = jsonObject["total_apartment"];
                                dtoAdmin.total_booking = jsonObject["total_booking"];
                                dtoAdmin.total_booking_finish = jsonObject["total_booking_finish"];
                            }

                            if (role == 1)
                            {

                                dtoChuKs.total_good_review = jsonObject["total_good_review"];
                                dtoChuKs.total_review = jsonObject["total_review"];
                                dtoChuKs.total_apartment_rented = jsonObject["total_apartment_rented"];
                                dtoChuKs.total = jsonObject["total"];
                                dtoChuKs.total_month = jsonObject["total_month"];
                                dtoChuKs.total_apartment = jsonObject["total_apartment"];
                                dtoChuKs.total_booking = jsonObject["total_booking"];
                                dtoChuKs.total_booking_finish = jsonObject["total_booking_finish"];
                                  
                            }
                        }

                    });
                    task.Wait();
                    if (Convert.ToInt32(Session["role"]) == 1)
                    {
                        return View("HomeChuKs",dtoChuKs);
                    }
                    else
                    {
                        return View(dtoAdmin);
                    }
                }
            }
            return RedirectToAction("Login", "Account");

        }
        public ActionResult HomeChuKs(DashBoardChuKs dtoChuKs)
        {

            return View(dtoChuKs);
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
        [ValidateInput(false)]
        public async Task<ActionResult> Create(Apartments apartments)
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


                }
            }


            return View();


        }
    }

}