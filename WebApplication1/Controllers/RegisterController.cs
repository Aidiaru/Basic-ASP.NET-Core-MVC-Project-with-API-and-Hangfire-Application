using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly LogService _logService;

        public RegisterController(IHttpClientFactory httpClientFactory, LogService logService)
        {
            _httpClientFactory = httpClientFactory;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Cache kontrolü - Register sayfası için de önbelleği devre dışı bırak
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            
            // Mevcut oturum varsa çıkış yap
            if (User.Identity?.IsAuthenticated == true)
            {
                string email = "Unknown";
                
                // JWT token'dan email bilgisini al
                var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim != null)
                {
                    email = emailClaim.Value;
                }
                
                var token = HttpContext.Session.GetString("JWToken");

                // Cookie'den çıkış yap
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Çıkış log kaydı
                await _logService.SendLogAsync(
                    new Log
                    {
                        Logger = "RegisterController",
                        Message = $"Register sayfası yüklendiğinde otomatik çıkış yapıldı: {email}",
                        Level = "Info",
                        Date = DateTime.UtcNow
                    },
                    token
                );

                HttpContext.Session.Clear();
                ViewBag.Message = "Önceki oturumunuz sonlandırıldı.";
            }

            return View();
        }

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
                // Başarılı kayıt log kaydı
                await _logService.SendLogAsync(
                    new Log
                    {
                        Logger = "RegisterController",
                        Message = $"Yeni kayıt başarılı: {model.Email}",
                        Level = "Info",
                        Date = DateTime.UtcNow
                    },
                    null
                );
                
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