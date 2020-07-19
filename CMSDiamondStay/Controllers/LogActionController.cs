using CMSDiamondStay.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMSDiamondStay.Controllers
{
    public class LogActionController : BaseController
    {
        // GET: LogAction
        public ActionResult Index(int? size, int? page, string searchString, string searchStringDate)
        {
            if (Session["Authent"] != null)
            {
                List<LogAction> LogActions = new List<LogAction>();
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "10", Value = "10" });
                items.Add(new SelectListItem { Text = "15", Value = "15" });
                items.Add(new SelectListItem { Text = "20", Value = "20" });
                items.Add(new SelectListItem { Text = "25", Value = "25" });
                items.Add(new SelectListItem { Text = "30", Value = "30" });
                items.Add(new SelectListItem { Text = "35", Value = "35" });
                ViewBag.CurrentFilter = searchString;
                ViewBag.CurrentFilterDate = searchStringDate;
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
                        HttpResponseMessage Res = await client.GetAsync("");

                        Res = await client.GetAsync($"/admin/users/action/logs?limit=1200&page=1");
                        if (searchString != null)
                        {
                            Res = await client.GetAsync($"/admin/users/action/logs?limit=500&key={searchString}&page=1");

                        }
                        if (searchStringDate != null)
                        {
                            Res = await client.GetAsync($"/admin/users/action/logs?limit=500&key={searchStringDate}&page=1");

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
                                    LogActions.Add(new LogAction()
                                    {
                                        id =item["id"],
                                        action = Convert.ToString(item["action"]),
                                        table_access = Convert.ToString(item["table_access"]),
                                        action_time = Convert.ToDateTime(item["action_time"]),
                                        ip = Convert.ToString(item["ip"])
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

                return View(LogActions.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Login", "Account");
        }
    }
}