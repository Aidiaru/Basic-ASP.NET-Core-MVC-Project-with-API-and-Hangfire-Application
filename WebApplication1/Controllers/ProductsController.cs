using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;

        public ProductsController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<IActionResult> Index()
        {
            // 1) JWT kontrolü
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Login");

            // 2) HttpClient + Bearer
            var client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7209/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 3) API çağrısı
            var response = await client.GetAsync("api/ProductApi");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
                }

                ViewBag.Message = "Ürünler alınamadı!";
                return View(new List<ProductDto>());
            }

            // 4) DTO’ya deserialize et
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            return View(products);
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string price)
        {
            // MVC tarafı validasyon
            if (string.IsNullOrWhiteSpace(name) || !int.TryParse(price, out int priceInt) || priceInt <= 0)
                return Json(new { success = false, message = "Fiyat için pozitif tamsayı giriniz." });

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Oturum geçersiz." });

            var client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7209/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var yeniUrun = new ProductDto { Name = name, Price = priceInt };
            var response = await client.PostAsJsonAsync("api/ProductApi", yeniUrun);

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, message = "Ürün başarıyla eklendi." });

            return Json(new { success = false, message = "İşlem başarısız." });
        }

        [HttpPost]
        public async Task<JsonResult> Edit(int id, string name, string price)
        {
            if (string.IsNullOrWhiteSpace(name) || !int.TryParse(price, out int priceInt) || priceInt <= 0)
                return Json(new { success = false, message = "Fiyat için pozitif tamsayı giriniz." });

            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Oturum geçersiz." });

            var client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7209/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var updated = new ProductDto { Id = id, Name = name, Price = priceInt };
            var response = await client.PutAsJsonAsync($"api/ProductApi/{id}", updated);

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, message = "Ürün başarıyla güncellendi." });

            return Json(new { success = false, message = "İşlem başarısız." });
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Oturum geçersiz." });

            var client = _httpFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7209/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"api/ProductApi/{id}");

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, message = "Ürün başarıyla silindi." });

            return Json(new { success = false, message = "İşlem başarısız." });
        }
    }
}