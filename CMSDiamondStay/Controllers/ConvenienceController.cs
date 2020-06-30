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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;

namespace CMSDiamondStay.Controllers
{
    public class ConvenienceController : Controller
    {
        string Baseurl = "http://35.197.153.19:12345";
        // GET: Convenience
        public async Task<ActionResult> Index(int? size, int? page)
        {
            List<ConvenienceViewModel> students = new List<ConvenienceViewModel>();
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

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync("/admin/convenience");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var jsonObject = serializer.Deserialize<dynamic>(EmpResponse)["data"];
                        foreach (var item in jsonObject)
                        {
                            students.Add(new ConvenienceViewModel() { id = item["id"], name = item["name"], description = item["description"] });
                        }



                        //Deserializing the response recieved from web api and storing into the Employee list  
                        //students = routes_list;

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


        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(ConvenienceViewModel model)
        {
            if (Session["Authent"] != null)
            {
               
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://35.197.153.19:12345/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["Authent"].ToString());
                    //HTTP POST
                    var response = client.PostAsync("admin/convenience", new StringContent(
       new JavaScriptSerializer().Serialize(model), Encoding.UTF8, "application/json"));

                   
                    //var result = postTask.Result;
                    if (response.Result.IsSuccessStatusCode)
                    {
                        TempData["message"] = "Create Success";
                        return RedirectToAction("Index", "Convenience");
                    }
                    
                }
            }
            return View();
        }
    }
}