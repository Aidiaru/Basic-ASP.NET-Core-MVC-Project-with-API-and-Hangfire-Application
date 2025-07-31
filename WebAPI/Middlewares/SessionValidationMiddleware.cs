using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;

    public SessionValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        // Sadece yetkili (Authorize) endpoint'ler için kontrol et
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var sessionId = context.User.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(sessionId))
            {
                var user = await db.Users.FindAsync(int.Parse(userId));
                if (user == null || user.SessionId.ToString() != sessionId)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = "Oturum geçersiz veya baþka bir yerde açýlmýþ." });
                    return;
                }
            }
        }

        await _next(context);
    }
}