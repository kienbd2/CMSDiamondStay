using CMSDiamondStay.Models;
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
    public class CampainController : Controller
    {
        // GET: Campain
        string Baseurl = "http://35.240.224.17:12345";
        public ActionResult Index(int? size, int? page)
        {

            List<Campaign> campaigns = new List<Campaign>();
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
                        HttpResponseMessage Res = await client.GetAsync($"/manager/campaign?page=1&limit=100");
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api   
                            var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"]["data"];
                            foreach (var item in jsonObject)
                            {
                                campaigns.Add(new Campaign()
                                {
                                    amount = Convert.ToInt32(item["amount"]),
                                    start_date = item["start_date"],
                                    end_date = item["end_date"],
                                    code = item["code"],
                                    percent = Convert.ToInt32(item["percent"]),
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
                return View(campaigns.ToPagedList(pageNumber, pageSize));

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
        public async Task<ActionResult> Create(Campaign campaign)
        {
            if (Session["Authent"] != null && Convert.ToInt32(Session["role"]) == 1)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    //HTTP POST
                    var response = await client.PostAsync("/manager/campaign/create", new StringContent(
   new JavaScriptSerializer().Serialize(campaign), Encoding.UTF8, "application/json"));

                    //HttpResponseMessage Res = await client.GetAsync("/users");
                    var EmpResponse = await response.Content.ReadAsStringAsync();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    int code = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["code"]);
                    int status = Convert.ToInt32(serializer.Deserialize<dynamic>(EmpResponse)["status"]);
                    if (code != 200)
                    {
                        return Json(new { result = false, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"], url = Url.Action("Index", "Campain") });
                    }
                    if (response.IsSuccessStatusCode && status == 1)
                    {
                        return Json(new { result = true, mess = serializer.Deserialize<dynamic>(EmpResponse)["message"], url = Url.Action("Index", "Campain") });
                    }

                    return Json(new { result = false, mess = "error create user" });
                }
            }
            return Json(new { mess = "Không đủ quyền", url = Url.Action("Index", "User") });
        }
    }
}