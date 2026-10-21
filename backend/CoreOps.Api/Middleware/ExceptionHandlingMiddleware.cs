using System.Net;
using System.Text.Json;
using CoreOps.Application.Common.Validation;
using CoreOps.Application.Common.Exceptions;

namespace CoreOps.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            var response = new
            {
                message = ex.Message,
                errors = ex.Errors
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            var response = new
            {
                message = ex.Message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
