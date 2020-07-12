using CMSDiamondStay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PagedList;
using System.Threading.Tasks;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using System.Text;
using Newtonsoft.Json;

namespace CMSDiamondStay.Controllers
{
    public class UserController : Controller
    {
        string Baseurl = "http://35.240.224.17:12345";
        // GET: User
        public ActionResult Index(int? size, int? page, string searchString)
        {


            List<User> students = new List<User>();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "9", Value = "9" });
            items.Add(new SelectListItem { Text = "12", Value = "12" });
            items.Add(new SelectListItem { Text = "15", Value = "15" });
            items.Add(new SelectListItem { Text = "18", Value = "18" });
            items.Add(new SelectListItem { Text = "30", Value = "30" });
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
                        HttpResponseMessage Res = await client.GetAsync($"/admin/users?page=1&limit=100&type=1");
                        if (!searchString.IsNullOrWhiteSpace())
                        {
                            Res = await client.GetAsync($"/admin/users?page=1&limit=100&key={searchString}&type=1");
                        }
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"]["data"];
                            foreach (var item in jsonObject)
                            {
                                students.Add(new User()
                                {
                                    user_id = item["user_id"],
                                    first_name = item["first_name"],
                                    last_name = item["last_name"],
                                    email = item["email"],
                                    is_activated = item["is_activated"],
                                    modifiedTime = DateTime.Parse(item["modifiedTime"]),
                                    first_time = item["first_time"],
                                    role = item["role"],
                                    main_balance = item["main_balance"],
                                });
                            }

                        }

                    });
                    task.Wait();
                }
                ViewBag.size = items; // ViewBag DropDownList
                ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
                page = page ?? 1;
                int pageSize = (size ?? 6);
                int pageNumber = (page ?? 1);
                return View(students.ToPagedList(pageNumber, pageSize));

            }
            return RedirectToAction("Login", "Account");

        }
        public ActionResult Admin(int? size, int? page, string searchString)
        {

            List<User> students = new List<User>();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "9", Value = "9" });
            items.Add(new SelectListItem { Text = "12", Value = "12" });
            items.Add(new SelectListItem { Text = "15", Value = "15" });
            items.Add(new SelectListItem { Text = "18", Value = "18" });
            items.Add(new SelectListItem { Text = "30", Value = "30" });
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
                        HttpResponseMessage Res = await client.GetAsync($"/admin/users?page=1&limit=100&type=2");
                        if (!searchString.IsNullOrWhiteSpace())
                        {
                            Res = await client.GetAsync($"/admin/users?page=1&limit=100&key={searchString}&type=1");
                        }
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"]["data"];
                            foreach (var item in jsonObject)
                            {
                                students.Add(new User()
                                {
                                    user_id = item["user_id"],
                                    first_name = item["first_name"],
                                    last_name = item["last_name"],
                                    email = item["email"],
                                    is_activated = item["is_activated"],
                                    modifiedTime = DateTime.Parse(item["modifiedTime"]),
                                    first_time = item["first_time"],
                                    role = item["role"],
                                    main_balance = item["main_balance"],
                                });
                            }

                        }

                    });
                    task.Wait();
                }
                ViewBag.size = items; // ViewBag DropDownList
                ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
                page = page ?? 1;
                int pageSize = (size ?? 6);
                int pageNumber = (page ?? 1);
                return View(students.ToPagedList(pageNumber, pageSize));

            }
            return RedirectToAction("Login", "Account");

        }

        public ActionResult Create()
        {

            if (Session["Authent"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public async Task<ActionResult> Create(User model)
        {
            if (Session["Authent"] != null && Convert.ToInt32(Session["role"]) == 2)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    //HTTP POST
                    var response = await client.PostAsync("/admin/users/create", new StringContent(
   new JavaScriptSerializer().Serialize(model), Encoding.UTF8, "application/json"));

                    //HttpResponseMessage Res = await client.GetAsync("/users");
                    var EmpResponse = await response.Content.ReadAsStringAsync();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    int code = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["code"]);
                    int status = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["status"]);
                    if (code != 200)
                    {
                        return Json(new { result = false, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"], url = Url.Action("Index", "User") });
                    }
                    if (response.IsSuccessStatusCode && status == 1)
                    {
                        return Json(new { result = true, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"], url = Url.Action("Index", "User") });
                    }

                    return Json(new { result = false, mess = "error create user" });
                }
            }
            return Json(new { mess = "Không đủ quyền", url = Url.Action("Index", "User") });

        }

        public ActionResult Index2()
        {
            return View();
        }

        public async Task<ActionResult> Detail(string id)
        {
            if (Session["Authent"] != null)
            {
                var user = new User();
                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.GetAsync($"admin/users/{id}");

                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var item = serializer.Deserialize<dynamic>(EmpResponse)["data"];

                        user.user_id = item["user_id"];
                        user.first_name = item["first_name"];
                        user.last_name = item["last_name"];
                        user.email = item["email"];
                        user.is_activated = item["is_activated"];
                        user.first_time = item["first_time"];
                        user.role = item["role"];
                        user.main_balance = item["main_balance"];
                        return View(user);
                    }
                }

            }
            return RedirectToAction("Login", "Account");
        }
        public class activeUser
        {
            public long user_id { get; set; }
            public bool status_active { get; set; }
        }
        public async Task<JsonResult> ChangeStatus(string id)
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
                    int code = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["code"]);
                    int status = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["status"]);
                    if (code != 200)
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

        public PartialViewResult GetPagingAsync(int? page)
        {
            if (Session["Authent"] != null)
            {
                int pageSize = 10;
                List<User> students = new List<User>();

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
                        HttpResponseMessage Res = await client.GetAsync($"/admin/users?page={page}");
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"]["data"];
                            foreach (var item in jsonObject)
                            {
                                students.Add(new User()
                                {
                                    user_id = item["user_id"],
                                    first_name = item["first_name"],
                                    last_name = item["last_name"],
                                    email = item["email"],
                                    is_activated = item["is_activated"],
                                    modifiedTime = DateTime.Parse(item["modifiedTime"]),
                                    first_time = item["first_time"],
                                    role = item["role"],
                                    main_balance = item["main_balance"],
                                });
                            }

                        }

                    });
                    task.Wait();
                    //Checking the response is successful or not which is sent using HttpClient  

                }


                int pagenumber = (page ?? 1);

                return PartialView("_PartialViewUser", students.ToPagedList(pagenumber, pageSize));
            }

            return PartialView();


        }

    }
}