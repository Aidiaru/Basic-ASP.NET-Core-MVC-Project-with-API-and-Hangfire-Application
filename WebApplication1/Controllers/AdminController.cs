using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Models;

[Authorize(Roles = "Admin")]
[Route("admin/[action]")]
public class AdminController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AdminController(IHttpClientFactory clientFactory)
        => _clientFactory = clientFactory;

    private string GetTokenOrRedirect()
    {
        var token = HttpContext.Session.GetString("JWToken");
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedAccessException();
        return token;
    }

    [HttpGet]
    public async Task<IActionResult> Logs(int page = 1, int pageSize = 10, string? search = null)
    {
        // 1. Token kontrolü
        string token;
        try
        {
            token = GetTokenOrRedirect();
        }
        catch
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. HttpClient hazırlanması
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7209/");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // 3. Query string oluşturma
        var uri = $"api/LogsApi?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
        {
            uri += $"&search={Uri.EscapeDataString(search)}";
        }

        // 4. API çağrısı
        var response = await client.GetAsync(uri);

        // 5. Hata kontrolü
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }

            ViewData["Error"] = "Log verileri alınamadı.";
            var emptyVm = new LogsViewModel
            {
                Logs = new List<Log>(),
                Page = page,
                PageSize = pageSize,
                TotalCount = 0,
                Search = search ?? string.Empty
            };
            return View(emptyVm);
        }

        // 6. Başarılı yanıtın işlenmesi
        var paged = await response.Content
            .ReadFromJsonAsync<PagedLogResult>(_jsonOptions);

        var vm = new LogsViewModel
        {
            Logs = paged?.Logs ?? new List<Log>(),
            Page = page,
            PageSize = pageSize,
            TotalCount = paged?.TotalCount ?? 0,
            Search = search ?? string.Empty
        };

        return View(vm);
    }


    [HttpGet]
    public async Task<IActionResult> Users()
    {
        string token;
        try { token = GetTokenOrRedirect(); }
        catch { return RedirectToAction("Index", "Login"); }

        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7209/");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("api/UserApi");
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            TempData["Error"] = "Kullanıcı listesi alınamadı.";
            return View(new List<UserViewModel>());
        }

        var users = await response.Content
            .ReadFromJsonAsync<List<UserViewModel>>(_jsonOptions)
            ?? new List<UserViewModel>();

        return View(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> EditUser(int id)
    {
        string token;
        try { token = GetTokenOrRedirect(); }
        catch { return RedirectToAction("Index", "Login"); }

        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7209/");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var resp = await client.GetAsync($"api/UserApi/{id}");
        if (!resp.IsSuccessStatusCode)
        {
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            TempData["Error"] = "Kullanıcı bilgisi alınamadı.";
            return RedirectToAction("Users");
        }

        var user = await resp.Content
            .ReadFromJsonAsync<UserViewModel>(_jsonOptions);
        return View(user);
    }

    [HttpPost("{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(UserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        string token;
        try { token = GetTokenOrRedirect(); }
        catch { return RedirectToAction("Index", "Login"); }

        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7209/");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var resp = await client.PutAsJsonAsync($"api/UserApi/{model.Id}", model);
        if (resp.IsSuccessStatusCode)
        {
            TempData["Success"] = "Kullanıcı başarıyla güncellendi.";
            return RedirectToAction("Users");
        }

        if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        var err = await resp.Content.ReadAsStringAsync();
        TempData["Error"] = $"Güncelleme başarısız: {err}";
        return View(model);
    }

    [HttpPost("{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(int id)
    {
        string token;
        try { token = GetTokenOrRedirect(); }
        catch { return RedirectToAction("Index", "Login"); }

        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7209/");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var resp = await client.DeleteAsync($"api/UserApi/{id}");
        if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        TempData[resp.IsSuccessStatusCode ? "Success" : "Error"] =
            resp.IsSuccessStatusCode ? "Kullanıcı silindi." : "Silme işlemi başarısız.";
        return RedirectToAction("Users");
    }
}