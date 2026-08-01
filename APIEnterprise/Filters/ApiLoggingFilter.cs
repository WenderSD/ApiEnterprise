using Microsoft.AspNetCore.Mvc.Filters;

namespace APIEnterprise.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger _logger;
        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("=====================================================");
            _logger.LogInformation(DateTime.Now.ToLongTimeString());
            _logger.LogInformation("=====================================================");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("=====================================================");
            _logger.LogInformation(DateTime.Now.ToLongTimeString());
            _logger.LogInformation("Status code: " + context.HttpContext.Response.StatusCode);
            _logger.LogInformation("=====================================================");
        }
    }
}
