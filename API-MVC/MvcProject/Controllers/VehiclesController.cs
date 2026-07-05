using Microsoft.AspNetCore.Mvc;
using MvcProject.Models;
using Newtonsoft.Json;
using System.Text;

namespace MvcProject.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly string apiUrl = "http://localhost:5234/api/Vehicles";

        [HttpGet]
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "/GetVehicle").Result;
            List<Vehicle> list = new List<Vehicle>();
            
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<List<Vehicle>>(response.Content.ReadAsStringAsync().Result);
            }
            
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Vehicle vehicle)
        {
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync(apiUrl + "/PostVehicle", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // For simplicity, we fetch all and find the one. Ideally there would be a GetById endpoint.
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "/GetVehicle").Result;
            List<Vehicle> list = JsonConvert.DeserializeObject<List<Vehicle>>(response.Content.ReadAsStringAsync().Result);
            var vehicle = list.FirstOrDefault(x => x.Id == id);
            
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        [HttpPost]
        public IActionResult Edit(Vehicle vehicle)
        {
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PutAsync(apiUrl + "/PutVehicle", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(apiUrl + "/DeleteVehicle/" + id).Result;

            return RedirectToAction("Index");
        }
    }
}
