using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = string.Join("<br/>",
                    ModelState.Values
                              .SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage));
                return View(model);
            }

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7209/");

            var response = await client.PostAsJsonAsync("api/AuthApi/register", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Kayıt başarılı! Giriş sayfasına yönlendiriliyorsunuz…";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = error;
                return View(model);
            }
        }
    }
}