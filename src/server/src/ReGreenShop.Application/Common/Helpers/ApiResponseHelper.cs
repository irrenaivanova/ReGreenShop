using Microsoft.AspNetCore.Mvc;

namespace ReGreenShop.Application.Common.Helpers;
public class ApiResponseHelper
{
    public static IActionResult Success(object data, string message = "Request successful")
    {
        return new OkObjectResult(new { status = 200, message = message, data = data });
    }

    public static IActionResult Created(object data, string message = "Resource created successfully")
    {
        return new CreatedResult("/resource", new { status = 201, message = message, data = data });
    }
}
