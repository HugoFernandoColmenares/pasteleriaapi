using Microsoft.AspNetCore.Diagnostics;
using Pasteleria.Shared.Extensions;
using System.Net;
using System.Text.Json;

namespace Pasteleria.Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var response = ApiResponse<object>.FailureResponse(
                "An unexpected error occurred on the server.",
                new List<string> { exception.Message },
                statusCode);

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(
                JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }),
                cancellationToken);

            return true;
        }
    }
}
