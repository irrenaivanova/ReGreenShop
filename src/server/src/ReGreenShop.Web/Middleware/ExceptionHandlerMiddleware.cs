using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReGreenShop.Application.Common.Exceptions;

namespace ReGreenShop.Web.Middleware;
public class ExceptionHandlerMiddleware
{
    // should be register

    //public async Task Invoke(HttpContext context)
    //{
    //    try
    //    {
    //        await _next(context);
    //    }
    //    catch (NotAuthorizedException ex)
    //    {
    //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Unhandled exception");
    //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    //        await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
    //    }
    // }
}
