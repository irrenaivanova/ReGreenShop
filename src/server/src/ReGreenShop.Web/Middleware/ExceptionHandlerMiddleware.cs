using System.Net;
using System.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReGreenShop.Application.Common.Exceptions;

namespace ReGreenShop.Web.Middleware;
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
        => this.next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    // check if all exceptions are here
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        var result = string.Empty;

        switch (exception)
        {
            case ModelValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonConvert.SerializeObject(validationException.Failures);
                break;
            case NotFoundException _:
                code = HttpStatusCode.NotFound;
                break;
            case AuthenticationException authEx:
                code = HttpStatusCode.BadRequest;
                result = JsonConvert.SerializeObject(new { error = authEx.Message });
                break;
            // 401 Unauthorized
            case UnauthorizedAccessException _:
                code = HttpStatusCode.Unauthorized;
                result = JsonConvert.SerializeObject(new { error = "Unauthorized: You must be authenticated to access this resource." });
                break;
            // 403 Forbidden
            case SecurityException _:
                code = HttpStatusCode.Forbidden;
                result = JsonConvert.SerializeObject(new { error = "Forbidden: You do not have permission to access this resource." });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (string.IsNullOrEmpty(result))
        {
            result = JsonConvert.SerializeObject(new { error = exception.Message });
        }

        return context.Response.WriteAsync(result);
    }
}

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<ExceptionHandlerMiddleware>();
}

