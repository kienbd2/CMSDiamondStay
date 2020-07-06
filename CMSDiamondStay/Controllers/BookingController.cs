using CMSDiamondStay.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

                    });
                    task.Wait();
                }

            }
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
            page = page ?? 1;
            int pageSize = (size ?? 10);
            int pageNumber = (page ?? 1);
            return View(bookings.ToPagedList(pageNumber, pageSize));
            
        }
    }
}