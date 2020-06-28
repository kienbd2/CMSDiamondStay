﻿using CMSDiamondStay.Models;
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

namespace CMSDiamondStay.Controllers
{
    public class UserController : Controller
    {
        string Baseurl = "http://35.197.153.19:12345";
        // GET: User
        public ActionResult Index(int? size, int? page)
        {
            List<User> students = new List<User>();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "9", Value = "9" });
            items.Add(new SelectListItem { Text = "12", Value = "12" });
            items.Add(new SelectListItem { Text = "15", Value = "15" });
            items.Add(new SelectListItem { Text = "18", Value = "18" });
            items.Add(new SelectListItem { Text = "30", Value = "30" });
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
                        HttpResponseMessage Res = await client.GetAsync("/admin/users?page=1");
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

            }
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
            page = page ?? 1;
            int pageSize = (size ?? 6);
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Index2()
        {
            return View();
        }
        public async Task<ActionResult> Index3Async(int? size, int? page)
        {
            List<User> students = new List<User>();
            List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "5", Value = "5" });
                items.Add(new SelectListItem { Text = "10", Value = "10" });
                items.Add(new SelectListItem { Text = "20", Value = "20" });
                items.Add(new SelectListItem { Text = "25", Value = "25" });
                items.Add(new SelectListItem { Text = "50", Value = "50" });
                items.Add(new SelectListItem { Text = "100", Value = "100" });
                items.Add(new SelectListItem { Text = "200", Value = "200" });
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
                    
                        HttpResponseMessage Res = await client.GetAsync("/admin/users?page=1");
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
                   

                }

            }
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại
            page = page ?? 1;
            int pageSize = (size ?? 5);
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
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