using Project.Common.Options;
using Microsoft.Extensions.Options;

namespace Project.API.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyMiddleware> _logger;
        private readonly AppSettings _appSettings;
        public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string apiKey = context.Request.Headers["X-Api-Key"].ToString();

            if(string.IsNullOrEmpty(apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var error = new
                {
                    statusCode = 401,
                    message = "API Key is missing"
                };

                await context.Response.WriteAsJsonAsync(error);
                return;
            }

            if(!_appSettings.ApiSettings.Keys.Values.Contains(apiKey))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var error = new
                {
                    StatusCode = 403,
                    Message = "Invalid API Key"
                };

                await context.Response.WriteAsJsonAsync(error);
                return;
            }

            await _next(context);
        }
    }
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKey(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
