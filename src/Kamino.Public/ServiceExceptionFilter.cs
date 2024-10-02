using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kamino.Public;

public class ServiceExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is BadRequestException)
        {
            context.Result = new BadRequestResult();
            context.ExceptionHandled = true;
        }
        else if (context.Exception is NotFoundException)
        {
            context.Result = new NotFoundResult();
            context.ExceptionHandled = true;
        }
        else if (context.Exception is ForbiddenException)
        {
            context.Result = new ForbidResult();
            context.ExceptionHandled = true;
        }
        else if (context.Exception is GoneException)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status410Gone);
            context.ExceptionHandled = true;
        }
    }
}
