using FluentValidation;
using AMCode.Web.Errors;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AMCode.Web.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions globally with standardized error responses
    /// </summary>
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Don't modify response if it has already started (CORS headers may have been set)
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse();
            int statusCode;

            if (exception is ValidationException validationEx)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Error = new ErrorDetail
                {
                    Code = "VALIDATION_ERROR",
                    Message = "Validation failed",
                    Details = validationEx.Errors.Select(e => new
                    {
                        PropertyName = e.PropertyName,
                        ErrorMessage = e.ErrorMessage,
                        AttemptedValue = e.AttemptedValue
                    }).ToArray()
                };
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Error = new ErrorDetail
                {
                    Code = "UNAUTHORIZED",
                    Message = "Unauthorized access",
                    Details = null
                };
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Error = new ErrorDetail
                {
                    Code = "INTERNAL_SERVER_ERROR",
                    Message = "An error occurred while processing your request",
                    Details = null
                };
            }

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
