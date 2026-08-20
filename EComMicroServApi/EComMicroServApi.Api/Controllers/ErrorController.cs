using Microsoft.AspNetCore.Mvc;

namespace EComMicroServApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult Error()
    {
        var exception = HttpContext.Features
        .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()
        ?.Error;

        if (exception is KeyNotFoundException)
        {
            return NotFound(new
            {
                message = exception.Message
            });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, new
        {
            message = "An unexpected error occurred."
        });
    }
}