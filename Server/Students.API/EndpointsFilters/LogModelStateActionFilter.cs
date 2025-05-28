using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Students.API.EndpointsFilters
{
    /// <summary>
    /// Логирование данных запроса, в случае ошибки при отправке Json
    /// </summary>
    // TODO: надо доделать
    public class LogModelStateActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogModelStateActionFilter> _logger;

        public LogModelStateActionFilter(ILogger<LogModelStateActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) 
        {
            if (context.HttpContext.Request.HasJsonContentType())
            {
                _logger.LogInformation($"Model validation failed for path: {context.HttpContext.Request.Path}");
                _logger.LogInformation($"Request Method: {context.HttpContext.Request.Method}");

                var headersValues = context.HttpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());

                _logger.LogInformation($"Request Headers: {JsonSerializer.Serialize(headersValues)}");
                var errors = context.ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                _logger.LogError($"Validation Errors (from ActionFilter): {JsonSerializer.Serialize(errors
                )}");

            }
            else
                _logger.LogInformation($"All Good");

            ActionExecutedContext resultContext = await next(); 

            // Логика ПОСЛЕ выполнения действия (OnActionExecuted)
            if (resultContext.Exception != null)
            {
                _logger.LogError(resultContext.Exception, "Async Filter: Exception occurred during action execution.");
            }
        }

    }
}