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

namespace CMSDiamondStay.Controllers
{
    public class UserController : Controller
    {
        string Baseurl = "http://35.197.153.19:12345";
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        [ChildActionOnly]
        public async Task<PartialViewResult> GetPagingAsync(int? page)
        {
            if (Session["Authent"] != null)
            {
                List<User> students = new List<User>();

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Session["Authent"].ToString());
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync("/admin/users?page=1&limit=20");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"];
                        foreach (var item in jsonObject)
                        {
                            students.Add(new User()
                            {
                                user_id = item["user_id"],
                                first_name = item["first_name"],
                                last_name = item["last_name"],
                                email = item["email"],
                                is_activated = item["is_activated"],
                                modifiedTime = item["modifiedTime"],
                                first_time = item["first_time"],
                                role = item["role"],
                                main_balance = item["main_balance"],
                            });
                        }

                    }

                }

                int pageSize = 10;
                int pagenumber = (page ?? 1);

                return PartialView("_PartialViewUser", students.ToPagedList(pagenumber, pageSize));
            }

            return PartialView();


        }

    }
}