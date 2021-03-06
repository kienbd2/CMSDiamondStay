﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CMSDiamondStay.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.WebParts;

namespace CMSDiamondStay.Controllers
{
    public class ApartmentController : BaseController
    {
        
        public ActionResult Index(int? size, int? page, string searchString)
        {
            List<ApartmentsViewModel> apartments = new List<ApartmentsViewModel>();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "15", Value = "15" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "30", Value = "30" });
            items.Add(new SelectListItem { Text = "35", Value = "35" });
            ViewBag.CurrentFilter = searchString;

            if (Session["Authent"] != null)
            {
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
                        HttpResponseMessage Res = await client.GetAsync($"/manager/apartments/?page=1&limit=100");
                        if (!searchString.IsNullOrWhiteSpace())
                        {
                            Res = await client.GetAsync($"/manager/apartments/?page=1&limit=100&key={searchString}");
                        }
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"]["data"];
                            foreach (var item in jsonObject)
                            {
                                apartments.Add(new ApartmentsViewModel()
                                {
                                    id = Convert.ToInt64(item["id"]),
                                    name = Convert.ToString(item["name"]),
                                    price = float.Parse(item["price"]),
                                    price_promotion = float.Parse(item["price_promotion"]),
                                    amount_bed = Convert.ToInt32(item["amount_bed"]),
                                    amount_sofa = Convert.ToInt32(item["amount_sofa"]),
                                    amount_bedroom = Convert.ToInt32(item["amount_bedroom"]),
                                    capacity_max = Convert.ToInt32(item["capacity_max"]),
                                    village_address = Convert.ToString(item["village"]),
                                    district_address = Convert.ToString(item["district"]),
                                    province_address = Convert.ToString(item["province"]),
                                    type = Convert.ToString(item["type"]),
                                    thumb = Convert.ToString(item["thumb"]),
                                    is_activated = item["is_activated"]
                                });
                            }

                        }

                    });
                    task.Wait();
                }

            }
            ViewBag.stt = 1;
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
            ViewBag.total = apartments.Count;
            page = page ?? 1;
            int pageSize = (size ?? 6);
            int pageNumber = (page ?? 1);
            return View(apartments.ToPagedList(pageNumber, pageSize));
        }
        // GET: Apartment
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
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, mess = "Not valid" });
            }

            var lstImage = new List<string>();
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 5)
            {
                return Json(new { result = false, mess = "Không up lớn hơn 5 ảnh", url = Url.Action("Index", "Apartment") });
            }

            Task task = Task.Run(async () =>
            {
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
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    lstImage.Add(uploadResult.SecureUrl.ToString());
                }
            });
            task.Wait();



            if (lstImage.Count <= 0)
            {
                return Json(new { result = false, mess = "Chưa up ảnh hoặc up ảnh không thành công", url = Url.Action("Index", "Apartment") });
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
            if (arrListStr.Count() < 4)
            {
                return Json(new { result = false, mess = $"Địa chỉ của bạn nhập là {apartments.travelfrom} không đúng định dạng VD: số 50 Cầu Giấy, Dịch Vọng, Cầu Giấy, Hà Nội. Xin vui lòng nhập lại", url = Url.Action("CreateApartment", "Apartment") });
            }
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





            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                //HTTP POST
                var response = await client.PostAsync("manager/apartments", new StringContent(
new JavaScriptSerializer().Serialize(apartment), Encoding.UTF8, "application/json"));
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var EmpResponse = await response.Content.ReadAsStringAsync();
                int code = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["status"]);
                //var result = postTask.Result;
                if (response.IsSuccessStatusCode && code == 1)
                {
                    //TempData["message"] = serializer.Deserialize<dynamic>(EmpResponse)["message"];
                    return Json(new { result = true, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"], url = Url.Action("Index", "Apartment") });

                }
                return Json(new { result = false, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"] });
            }

        }
        public class activePartment
        {
            public long apartment_id { get; set; }
            public bool status_active { get; set; }
        }
        public async Task<JsonResult> ChangeStatus(string id, bool active)
        {
            if (Session["Authent"] != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var activePartment = new activePartment();
                    activePartment.apartment_id = long.Parse(id);
                    activePartment.status_active = active;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    //HTTP POST
                    var response = await client.PostAsync("manager/apartments/toggle-active", new StringContent(
   new JavaScriptSerializer().Serialize(activePartment), Encoding.UTF8, "application/json"));

                    //HttpResponseMessage Res = await client.GetAsync("/users");
                    var EmpResponse = await response.Content.ReadAsStringAsync();
                    int code2 = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["code"]);
                    int status = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["status"]);
                    if (code2 != 200)
                    {
                        return Json(new { result = false, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"] });
                    }
                    if (response.IsSuccessStatusCode && status == 1)
                    {
                        return Json(new { result = active, status = active, mess = "Thêm phòng thành công" });
                    }

                    return Json(new { result = false, mess = "Lỗi xác nhận thanh toán" });
                }
            }
            return Json(new { result = false, mess = "ko co quyen" });

        }
        public class Convenience
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        public class ListConvenience
        {
            public List<Convenience> lstConvenience { get; set; }
        }

        public async Task<ActionResult> Edit(long id)
        {
            var apartment = new ApartmentsViewModel();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"/client/apartment/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var item = serializer.Deserialize<dynamic>(EmpResponse)["data"];
                    apartment.id =Convert.ToInt64(id);
                    apartment.name = item["name"]; //Reading text box values using Jquery
                    apartment.amount_bed = item["amount_bed"];
                    apartment.amount_sofa = item["amount_sofa"];
                    apartment.capacity_standard = item["capacity_standard"];
                    apartment.capacity_max = item["capacity_max"];
                    apartment.relax_suggest = item["relax_suggest"];
                    apartment.direction_instruction = item["direction_instruction"];
                    apartment.cuisine_suggest = item["cuisine_suggest"];
                    apartment.regulation = item["regulation"];
                    apartment.phone = Convert.ToString( item["phone"]);
                    apartment.area = Convert.ToDouble(item["area"]);
                    apartment.amount_bathroom = item["amount_bathroom"];
                    apartment.amount_bedroom = item["amount_bedroom"];
                    apartment.star_standard = item["star_standard"];
                    var str = Newtonsoft.Json.JsonConvert.SerializeObject(item["conveniences"]);

                    //var result = JsonConvert.DeserializeObject<ListConvenience>(str);
                    List<Convenience> item2 = serializer.Deserialize<List<Convenience>>(str);
                    apartment.conveniences = item2.Select(x => x.id).ToList();
                    //apartment.cancel_policy = Convert.ToInt32(item["cancel_policy"]);
                    //apartment.latitude = item["latitude"];
                    //apartment.longitude = item["longitude"];
                    apartment.detail_address = item["detail_address"];
                    apartment.village_address = item["village_address"];
                    apartment.district_address = item["district_address"];
                    apartment.province_address = item["province_address"];
                    //apartment.apartment_type =1* Convert.ToInt32(item["apartment_type"]);
                    apartment.price = Convert.ToDouble(item["price"]);
                    apartment.price_promotion = Convert.ToDouble(item["price_promotion"]);
                    apartment.min_day = item["min_day"];
                    apartment.max_day = item["max_day"];
                    apartment.surcharge_per_person = Convert.ToDouble(item["surcharge_per_person"]);
                    apartment.check_in_time = item["check_in_time"];
                    apartment.check_out_time = item["check_out_time"];
                    apartment.description = item["description"];
                    var str2 = Newtonsoft.Json.JsonConvert.SerializeObject(item["gallery"]);
                    apartment.gallery = serializer.Deserialize<List<string>>(str2);

                }
            }
            ViewBag.convenience = getAllConvenience();
            return View(apartment);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(ApartmentEdit apartments)
        {

            var lstImage = new List<string>();
            HttpFileCollectionBase files = Request.Files;
            if (files.Count > 5)
            {
                return Json(new { result = false, mess = "Không up lớn hơn 5 ảnh", url = Url.Action("Index", "Apartment") });
            }
            if (files.Count > 1)
            {
                Task task = Task.Run(async () =>
                {
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
                        var uploadResult = await cloudinary.UploadAsync(uploadParams);
                        lstImage.Add(uploadResult.SecureUrl.ToString());
                    }
                });
                task.Wait();
            }
            

            var apartment = new ApartmentsViewModel();
            apartment.id = apartments.id;
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
            apartment.area = apartments.area;
            apartment.amount_bathroom = apartments.amount_bathroom;
            apartment.amount_bedroom = apartments.amount_bedroom;
            apartment.star_standard = apartments.star_standard;
            if (apartments.conveniencesEdit.Count > 1)
            {
                apartments.conveniencesEdit.RemoveAt(apartments.conveniencesEdit.Count - 1);
            }
            apartment.conveniences = apartments.conveniencesEdit;
            apartment.cancel_policy = apartments.cancel_policy;
            apartment.latitude = apartments.latitude;
            apartment.longitude = apartments.longitude;
            string[] arrListStr = apartments.travelfrom.Split(',');
            if (arrListStr.Count() < 4)
            {
                return Json(new { result = false, mess = $"Địa chỉ của bạn nhập là {apartments.travelfrom} không đúng định dạng VD: số 50 Cầu Giấy, Dịch Vọng, Cầu Giấy, Hà Nội. Xin vui lòng nhập lại", url = Url.Action("CreateApartment", "Apartment") });
            }
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





            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                //HTTP POST
                var response = await client.PostAsync("/manager/apartments/edit", new StringContent(
new JavaScriptSerializer().Serialize(apartment), Encoding.UTF8, "application/json"));
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var EmpResponse = await response.Content.ReadAsStringAsync();
                int code = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["status"]);
                //var result = postTask.Result;
                if (response.IsSuccessStatusCode && code == 1)
                {
                    //TempData["message"] = serializer.Deserialize<dynamic>(EmpResponse)["message"];
                    return Json(new { result = true, mess = "Sửa phòng thành công", url = Url.Action("Index", "Apartment") });

                }
                return Json(new { result = false, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"] });
            }

        }
    }
}