using CMSDiamondStay.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
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
    public class BookingController : Controller
    {
        string Baseurl = "http://35.197.153.19:12345";
        // GET: Booking
        public ActionResult Index(int? size, int? page, string searchString)
        {
            List<Booking> bookings = new List<Booking>();
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
                        HttpResponseMessage Res = await client.GetAsync($"/admin/booking/?page=1&limit=100");
                        if (!searchString.IsNullOrWhiteSpace())
                        {
                            Res = await client.GetAsync($"/admin/booking/?page=1&limit=30&key={searchString}");
                        }
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            int code = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["code"]);
                            if (code == 200)
                            {
                                var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"]["data"];
                                foreach (var item in jsonObject)
                                {
                                    bookings.Add(new Booking()
                                    {
                                        id = Convert.ToString(item["id"]),
                                        user_id = Convert.ToString(item["user_id"]),
                                        user_name = Convert.ToString(item["user_name"]),
                                        code = Convert.ToString(item["code"]),
                                        apartment_id = Convert.ToString(item["apartment_id"]),
                                        apartment_name = Convert.ToString(item["apartment_name"]),
                                        apartment_thumb = Convert.ToString(item["apartment_thumb"]),
                                        apartment_type = Convert.ToString(item["apartment_type"]),
                                        check_in = DateTime.Parse(item["check_in"]),
                                        check_out = DateTime.Parse(item["check_out"]),
                                        status = Convert.ToInt32(item["status"]),
                                        amount = Convert.ToString(item["amount"]),
                                    });
                                }
                            }
                        }

                    });
                    task.Wait();
                }

                ViewBag.size = items; // ViewBag DropDownList
                ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
                page = page ?? 1;
                int pageSize = (size ?? 10);
                int pageNumber = (page ?? 1);
                return View(bookings.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Account");

        }
        public class activeUser
        {
            public long user_id { get; set; }
            public bool status_active { get; set; }
        }
        public async Task<JsonResult> ChangeStatus(string id, string code, bool active)
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

                    var userActive = new activeUser();
                    userActive.user_id = long.Parse(id);

                    HttpResponseMessage Res = await client.GetAsync($"admin/users/{id}");
                    var EmpResponse2 = Res.Content.ReadAsStringAsync().Result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var item = serializer.Deserialize<dynamic>(EmpResponse2)["data"];
                    userActive.status_active = !item["is_activated"];
                    //HTTP POST
                    var response = await client.PostAsync("admin/users/toggle-active", new StringContent(
   new JavaScriptSerializer().Serialize(userActive), Encoding.UTF8, "application/json"));

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
                        return Json(new { result = true, status = userActive.status_active, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"] });
                    }

                    return Json(new { result = false, mess = "error create user" });
                }
            }
            return Json(new { result = false, mess = "ko co quyen" });

        }
    }
}