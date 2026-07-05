using Microsoft.AspNetCore.Mvc;
using MvcProject.Models;
using Newtonsoft.Json;
using System.Text;

namespace MvcProject.Controllers
{
    public class DriversController : Controller
    {
        private readonly string apiUrl = "http://localhost:5234/api/Drivers";

        [HttpGet]
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "/GetDriver").Result;
            List<Driver> list = new List<Driver>();
            
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<List<Driver>>(response.Content.ReadAsStringAsync().Result);
            }
            
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Driver driver)
        {
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(driver);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync(apiUrl + "/PostDriver", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(driver);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "/GetDriver").Result;
            List<Driver> list = JsonConvert.DeserializeObject<List<Driver>>(response.Content.ReadAsStringAsync().Result);
            var driver = list.FirstOrDefault(x => x.Id == id);
            
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        [HttpPost]
        public IActionResult Edit(Driver driver)
        {
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(driver);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PutAsync(apiUrl + "/PutDriver", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(driver);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(apiUrl + "/DeleteDriver/" + id).Result;

            return RedirectToAction("Index");
        }
    }
}
