using CarbonTodo.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarbonTodo.Api.ActionFilters;

public class AppExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is NotFoundEntityAppException)
        {
            context.Result = new NotFoundResult();
            context.ExceptionHandled = true;
        }

        if (context.Exception is ConflictingOrderAppException)
        {
            context.Result = new ConflictResult();
            context.ExceptionHandled = true;
        }
    }
}