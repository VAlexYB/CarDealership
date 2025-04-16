using CarDealership.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace CarDealership.Web.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            context.Response.ContentType = "application/json";

            if (exception is ClientInformationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(exception.Message));
            }
            else
            {
                logger.LogError(exception, "Unhandled exception occured.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize("Внутренняя ошибка сервера. Пожалуйста, попробуйте позже."));
            }
        }
    }
}
