using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // 1) [AllowAnonymous] varsa filtreyi atla
        if (IsAllowAnonymous(context))
            return;

        var session = context.HttpContext.Session;
        var token = session.GetString("JWToken");
        var userRole = session.GetString("UserRole");

        // 2) Login, Register veya Home için serbest bırak
        var ctrl = context.ActionDescriptor.RouteValues["controller"];
        var act = context.ActionDescriptor.RouteValues["action"];
        if (ctrl == "Login" || ctrl == "Register" || ctrl == "Home")
            return;

        // 3) Token yoksa → login’e
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
            return;
        }

        // 4) Admin sayfaları için rol kontrolü
        if (ctrl == "Admin" && userRole != "Admin")
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }

    bool IsAllowAnonymous(AuthorizationFilterContext ctx)
    {
        if (ctx.ActionDescriptor is ControllerActionDescriptor cad)
        {
            var onMethod = cad.MethodInfo
                                  .IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            var onController = cad.ControllerTypeInfo
                                  .IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            return onMethod || onController;
        }
        return false;
    }
}