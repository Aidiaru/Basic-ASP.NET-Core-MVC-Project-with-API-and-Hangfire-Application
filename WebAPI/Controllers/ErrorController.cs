using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpOptions, HttpHead, HttpPatch]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            return Problem(
                detail: exception?.Message,
                statusCode: 500,
                title: "Sunucu Hatasý"
            );
        }
    }
}