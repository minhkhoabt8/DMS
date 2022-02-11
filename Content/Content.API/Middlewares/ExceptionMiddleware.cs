using System.Text.Json;
using Content.API.ResponseWrapper;
using Content.Core.Exceptions.Common;
using Serilog;

namespace Content.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        int statusCode;
        object response;

        switch (exception)
        {
            // Expected exceptions in VOL.Core/Exceptions
            case HandledException handledEx:
                statusCode = handledEx.StatusCode;
                response = new ApiResponse(handledEx.StatusCode, isError: true, message: exception.Message);
                break;
            // Unexpected exceptions
            default:
                Log.Error(exception.Message);
                Log.Error(exception.ToString());
                statusCode = StatusCodes.Status500InternalServerError;
                response = new ApiInternalServerErrorResponse(exception.ToString());
                //response = new ApiInternalServerErrorResponse("Internal server error");
                break;
        }

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(
            response,
            new JsonSerializerOptions
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
    }
}