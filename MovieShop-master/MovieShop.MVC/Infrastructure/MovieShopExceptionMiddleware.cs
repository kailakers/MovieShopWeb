using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieShop.Core.Exceptions;

namespace MovieShop.MVC.Infrastructure
{
    //Note that the order in which you register middleware in your application’s request pipeline matters.The various middleware components execute sequentially
    //in the order in which they’re registered. In the case of a global exception handler like our ErrorHandlingMiddleware, we’ll register it early in the Configure(..)
    //method so that it captures exceptions that occur in downstream middleware components.


    public class MovieShopExceptionMiddleware
    {
        private readonly ILogger<MovieShopExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        public static readonly object HttpContextItemsMiddlewareKey = new Object();

        public MovieShopExceptionMiddleware(RequestDelegate next, ILogger<MovieShopExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("My MovieShopExceptionMiddleware BEFORE");
            try
            {
                _logger.LogInformation("My MovieShopExceptionMiddleware went through");
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Catching exception in ExceptionMiddleware: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        //    await _next(httpContext);
             _logger.LogInformation("My MovieShopExceptionMiddleware AFTER");
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
          //  context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            _logger.LogInformation($"Request completed with status code: {context.Response.StatusCode} ");
            _logger.LogError($"Something went wrong: {exception}");
            var errorDetails = new ErrorResponseModel
                               {
                                   ExceptionMessage = exception.Message,
                                   ExceptionStackTrace = exception.StackTrace,
                                   InnerExceptionMessage = exception.InnerException?.Message
                               };
            context.Items.Add("ErrorDetails", errorDetails);
            switch (exception)
            {
                case BadRequestException _:
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;

                case NotFoundException _:
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;

                case UnauthorizedAccessException _:
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    break;

                case ConflictException ex:
                    context.Response.StatusCode = (int) HttpStatusCode.Conflict;
                    break;

                case Exception e:
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.Redirect("/Home/Error");
            await Task.CompletedTask;

        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseMovieShopExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MovieShopExceptionMiddleware>();
        }
    }
}