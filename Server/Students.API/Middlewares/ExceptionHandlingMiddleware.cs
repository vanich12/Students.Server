using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Hosting; 
using System.Diagnostics; 

namespace Students.API.Middlewares
{
    /// <summary>
    /// Обработка ошибок для клиента
    /// </summary>
    public class ExceptionHandlingMiddleware : IMiddleware 
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment; 


        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context); 
            }
            catch (Exception ex)
            {
              
                _logger.LogError(ex, "An unhandled exception occurred while processing the request. Path: {Path}, TraceId: {TraceId}",
                    context.Request.Path, Activity.Current?.Id ?? context.TraceIdentifier);

          
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error page/middleware will not be executed.");
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected server error occurred.",
                    Instance = context.Request.Path,
                };

                // Добавляем TraceId для корреляции
                problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier;

                // В режиме разработки добавляем больше деталей
                if (_environment.IsDevelopment())
                {
                    problemDetails.Detail = ex.ToString(); // Включаем полный стектрейс и детали исключения
                }
                else
                {
                    problemDetails.Detail = "An internal error occurred while processing your request. Please try again later.";
                }

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}