using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MovieShop.API.Infrastructure
{
    public class ModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                                     {
                                         Instance = context.HttpContext.Request.Path,
                                         Status = StatusCodes.Status400BadRequest,
                                         Type = $"https://httpstatuses.com/400",
                                         Detail = "Please Refer to documentation"
                                     };

                context.Result = new BadRequestObjectResult(problemDetails);

            }
        }
    }
}