using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text.Json;
using TaskTracker.Common.Exceptions;

namespace TaskTracker.Web.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (BadRequestException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            string message = ex.Message;
            logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(httpContext, message, ex.IdentityErrors);
        }
        catch (ForbiddenException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            string message = ex.Message;
            logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(httpContext, message, null);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";
            logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(httpContext, message, null);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string message, IEnumerable<IdentityError>? errors)
    {
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(new ExceptionDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
            IdentityErrors = errors
        }.ToString());
    }
}

public record ExceptionDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public IEnumerable<IdentityError>? IdentityErrors { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}