using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogoAPI.Filter;

public class ApiLoggingFilter(ILogger<ApiLoggingFilter> logger) : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger = logger;
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("### Executing -> OnActionExecuting ###");
        _logger.LogInformation("######################################");
        _logger.LogInformation($"{DateTime.Now:T}");
        _logger.LogInformation($"Model State: {context.ModelState.IsValid}");
        _logger.LogInformation("######################################");

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("### Executed -> OnActionExecuted ###");
        _logger.LogInformation("######################################");
        _logger.LogInformation($"{DateTime.Now:T}");
        _logger.LogInformation($"Status Code: {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("######################################");
    }
}