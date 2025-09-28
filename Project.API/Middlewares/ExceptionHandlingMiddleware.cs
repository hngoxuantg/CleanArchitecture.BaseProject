using Project.Application.Exceptions;
using Project.Common.Constants;
using System.Text.Json;

namespace Project.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(context, ex);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception ex)
        {
            int statusCode;
            string errorType = ex.GetType().Name;
            object response;

            if (ex is ValidatorException validatorException)
            {
                _logger.LogWarning(ex, "Validation error occurred. Errors: {@ValidationErrors}",
                    validatorException.ValidationErrors);

                statusCode = (int)validatorException.HttpStatusCode;

                if (validatorException.ValidationErrors.Any())
                {
                    response = new
                    {
                        success = false,
                        errors = validatorException.ValidationErrors,
                        error = new
                        {
                            code = validatorException.ErrorCode,
                            type = errorType,
                        },
                        timestamp = validatorException.Timestamp
                    };
                }
                else
                {
                    response = new
                    {
                        success = false,
                        message = validatorException.Message,
                        error = new
                        {
                            code = validatorException.ErrorCode,
                            type = errorType,
                        },
                        timestamp = validatorException.Timestamp
                    };
                }
            }
            else if (ex is BaseCustomException baseCustomException)
            {
                _logger.LogWarning(ex, "Custom error occurred. ErrorCode: {ErrorCode}, ErrorType: {ErrorType}, Message: {Message}",
                    baseCustomException.ErrorCode, baseCustomException.ErrorType, baseCustomException.Message);

                statusCode = (int)baseCustomException.HttpStatusCode;

                response = new
                {
                    success = false,
                    message = baseCustomException.Message,
                    error = new
                    {
                        code = baseCustomException.ErrorCode,
                        type = errorType
                    },
                    timestamp = baseCustomException.Timestamp
                };
            }
            else
            {
                _logger.LogError(ex, "System error occurred: {Message}", ex.Message);

                statusCode = 500;

                response = new
                {
                    success = false,
                    message = ErrorMessages.SystemError,
                    error = new
                    {
                        code = "UNKNOWN",
                        type = errorType
                    },
                    timestamp = DateTime.UtcNow
                };
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            await context.Response.WriteAsync(json);
        }
    }
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}