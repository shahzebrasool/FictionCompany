using FictionalCompany.Dashboard.Models;
using FictionalCompany.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;

namespace FictionalCompany.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _config;
        private readonly string APIEndPoint;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, IConfiguration config)
        {
            _logger = logger;
            _httpClient = httpClient;
            _config = config;
            APIEndPoint = _config.GetSection("APIURL").GetSection("url").Value;

        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(APIEndPoint + "GetUsers");
            var content = await response.Content.ReadAsStringAsync();
            var usersList = JsonConvert.DeserializeObject<List<User>>(content);

            return View(usersList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var jsonContent = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                try
                {

                    var response = await httpClient.PostAsync(APIEndPoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "API request failed");

                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);

                }
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{APIEndPoint}{id}");
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(content);
                    return View(user);
                }
                catch (Exception ex)
                {
                    return Content("Something went wrong");
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            try
            {
                int id = user.Id;
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync($"{APIEndPoint}{id}",
                        new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "API request failed");

                    }

                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{APIEndPoint}{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "API request failed");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }

        }
    }
}