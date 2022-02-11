﻿using Auth.API.ResponseWrapper;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.API.Filters;

public class AutoValidateModelState : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = ResponseFactory.BadRequest(context.ModelState);
        }
    }
}