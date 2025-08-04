using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

public class HomeController : Controller
{
    public HomeController()
    {
        // Constructor'da çalýþacak
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Tüm Action'lar çalýþmadan önce cache kontrolü ekle
        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
        
        base.OnActionExecuting(context);
    }

    public IActionResult Index()
    {
        // Daha kapsamlý oturum kontrolü
        if (!User.Identity.IsAuthenticated || 
            string.IsNullOrEmpty(HttpContext.Session.GetString("JWToken")) ||
            string.IsNullOrEmpty(HttpContext.Session.GetString("SessionId")))
        {
            // Session veya cookie durumu tutarsýz - güvenli çýkýþ yap
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index", "Login");
        }
        
        // Ek güvenlik: Session ve cookie deðerlerini karþýlaþtýr
        var sessionId = HttpContext.Session.GetString("SessionId");
        var sessionIdClaim = User.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value;
        
        if (sessionId != sessionIdClaim)
        {
            // Session ve cookie deðerleri uyuþmuyor - çýkýþ yap
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index", "Login");
        }
        
        return View();
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}