using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Ap.Demo.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (status, message) = MapException(ex);
                context.Response.StatusCode = status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponseInfo
                {
                    StatusCode = status,
                    Message = message
                }));
            }
        }

        private static (int status, string message) MapException(Exception ex)
        {
            return ex switch
            {
                ValidationException fv => (StatusCodes.Status400BadRequest, fv.Message),
                _ => (StatusCodes.Status500InternalServerError, ex.Message)
            };
        }
    }

    public class ErrorResponseInfo
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}