using CMSDiamondStay.Models;
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
    public class BaseController : Controller
    {


        string Baseurl = "http://35.197.153.19:12345";
        public List<ConvenienceViewModel> getAllConvenience()
        {
            List<ConvenienceViewModel> students = new List<ConvenienceViewModel>();

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
                    });
                    task.Wait();
                }


            }
            return students;
        }
         
    }
}