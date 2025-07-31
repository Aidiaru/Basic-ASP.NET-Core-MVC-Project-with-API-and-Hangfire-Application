using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly LogService _logService;

        public LoginController(IHttpClientFactory clientFactory, LogService logService)
        {
            _clientFactory = clientFactory;
            _logService = logService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen gerekli alanları doldurun.";
                return View(model);
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7209/");
            var response = await client.PostAsJsonAsync("api/AuthApi/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Geçersiz e-posta veya şifre.";
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            // JWT ve kullanıcı bilgilerini session'a ekle
            HttpContext.Session.SetString("JWToken", result.Token);
            HttpContext.Session.SetString("UserRole", result.User.Role);
            HttpContext.Session.SetString("SessionId", result.SessionId);

            // JWT'den claims'leri çıkar
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(result.Token);
            
            // Claims Principal oluştur
            var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            
            // Cookie Authentication ile giriş yap
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                });

            // Başarılı giriş log kaydı
            await _logService.SendLogAsync(
                new Log
                {
                    Logger = "LoginController",
                    Message = $"Giriş başarılı: {model.Email}",
                    Level = "Info",
                    Date = DateTime.UtcNow
                },
                result.Token
            );

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var email = User.Identity?.Name ?? "Unknown";
            var token = HttpContext.Session.GetString("JWToken");

            // Cookie'den de çıkış yap
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Çıkış log kaydı
            await _logService.SendLogAsync(
                new Log
                {
                    Logger = "LoginController",
                    Message = $"Çıkış yaptı: {email}",
                    Level = "Info",
                    Date = DateTime.UtcNow
                },
                token
            );

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}