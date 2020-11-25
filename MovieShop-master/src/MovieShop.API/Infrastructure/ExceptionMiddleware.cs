using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieShop.Core.Exceptions;

//using Newtonsoft.Json;

namespace MovieShop.API.Infrastructure
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in ExceptionMiddleware: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            _logger.LogInformation($"Request completed with status code: {context.Response.StatusCode} ");
            _logger.LogError($"Something went wrong: {exception}");
            object errors = null;

            switch (exception)
            {
                case BadRequestException ex:
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    errors = ex.Message;
                    break;
                case ConflictException ex:
                    context.Response.StatusCode = (int) HttpStatusCode.Conflict;
                    errors = ex.Message;
                    break;
                case NotFoundException _:
                    // context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errors = exception.Message;
                 break;
                case HttpException re:
                    errors = re.Errors;
                    context.Response.StatusCode = (int) re.Code;
                    break;
                case Exception e:
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : "Server error, please try later";
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new {errors});
            await context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}