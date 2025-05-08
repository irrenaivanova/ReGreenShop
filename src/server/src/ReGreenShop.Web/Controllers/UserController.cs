using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Categories.Queries.GetRootCategories;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Common.Identity.Login;
using ReGreenShop.Application.Common.Identity.Register;

namespace ReGreenShop.Web.Controllers;
public class UserController : BaseController
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result, "Login successful");
    }


    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Created(result, "Registration successful, you can now log in.");
    }
}
