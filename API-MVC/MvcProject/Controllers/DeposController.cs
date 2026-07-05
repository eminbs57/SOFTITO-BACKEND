using Microsoft.AspNetCore.Mvc;
using MvcProject.Models;
using Newtonsoft.Json;
using System.Text;

namespace MvcProject.Controllers
{
    public class DeposController : Controller
    {
        private readonly string apiUrl = "http://localhost:5234/api/Depos";

        [HttpGet]
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "/GetDepo").Result;
            List<Depo> list = new List<Depo>();

            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<List<Depo>>(response.Content.ReadAsStringAsync().Result);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Depo depo)
        {
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(depo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync(apiUrl + "/PostDepo", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(depo);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(apiUrl + "/GetDepo").Result;
            List<Depo> list = JsonConvert.DeserializeObject<List<Depo>>(response.Content.ReadAsStringAsync().Result);
            var depo = list.FirstOrDefault(x => x.Id == id);

            if (depo == null)
            {
                return NotFound();
            }

            return View(depo);
        }

        [HttpPost]
        public IActionResult Edit(Depo depo)
        {
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(depo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PutAsync(apiUrl + "/PutDepo", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(depo);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(apiUrl + "/DeleteDepo/" + id).Result;

            return RedirectToAction("Index");
        }
    }
}
