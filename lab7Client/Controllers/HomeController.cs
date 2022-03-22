using lab7Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace lab7Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private string serviceUrl = "http://localhost/RestaurantReviewServiceApi/RestaurantReview";
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();

            using var response = client.GetAsync(serviceUrl).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                List<RestaurantInfo> restaurantInfos = JsonConvert.DeserializeObject<List<RestaurantInfo>>(apiResponse);

                return View(restaurantInfos);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(RestaurantInfo restaurantInfo)
        {
            HttpClient httpClient = new HttpClient();
            string restaurantInfoJsonString = JsonConvert.SerializeObject(restaurantInfo);
            StringContent content = new StringContent(restaurantInfoJsonString, UnicodeEncoding.UTF8, "application/json");

            using var response = httpClient.PostAsync(serviceUrl, content).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id ==null)
            {
                return NotFound();
            }
            
            HttpClient client = new HttpClient();
            using var response = client.GetAsync(serviceUrl +"/"+ id.Value).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string apiResponse = response.Content.ReadAsStringAsync().Result;
                RestaurantInfo restaurantInfo = JsonConvert.DeserializeObject<RestaurantInfo>(apiResponse);

                return View(restaurantInfo);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult Edit(RestaurantInfo restaurantInfo)
        {
            HttpClient httpClient = new HttpClient();
            string restaurantInfoJsonString = JsonConvert.SerializeObject(restaurantInfo);
            StringContent content = new StringContent(restaurantInfoJsonString, UnicodeEncoding.UTF8, "application/json");

            using var response = httpClient.PutAsync(serviceUrl + "/" + restaurantInfo.Id.ToString(), content).Result;

            if (response.StatusCode ==System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public IActionResult Delete(int? id)
        {
            HttpClient client = new HttpClient();
            using var response = client.DeleteAsync(serviceUrl + "/" + id.Value).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }      
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