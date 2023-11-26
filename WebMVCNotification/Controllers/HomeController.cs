using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;
using WebMVCNotification.Models;

namespace WebMVCNotification.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private string AppId { get; } = "APP-ID";
        private string ApiKey { get; } = "API-KEY";
        private string ApiUrl { get; } = "https://onesignal.com/api/v1/notifications";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendPushNotification()
        {
            try
            {


                var webRequest = WebRequest.Create($"{ApiUrl}") as HttpWebRequest;
                webRequest.KeepAlive = true;
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json; charset=utf-8";
                webRequest.Headers.Add("authorization", $"Basic {ApiKey}");

                var obj = new
                {
                    app_id = AppId,
                    headings = new { en = "Web Push Notification Title" }, //this value can be change as per need, can be a value from db
                    contents = new { en = "Here it goes push notification content" }, //this value can be change as per need, can be a value from db
                    included_segments = new string[] { "All" },
                    url = ""
                };
                var param = JsonConvert.SerializeObject(obj);
                var byteArray = Encoding.UTF8.GetBytes(param);

                using (var writer = webRequest.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = webRequest.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        using var reader = new StreamReader(response.GetResponseStream());
                        var responseContent = reader.ReadToEnd();
                        //return !responseContent.Contains("error");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View("~/views/home/index.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}