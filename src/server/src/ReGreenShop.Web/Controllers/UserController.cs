using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Common.Identity.Login;
using ReGreenShop.Application.Common.Identity.Register;
using ReGreenShop.Application.GreenAlternatives.GetTotalGreenImpact;
using ReGreenShop.Application.Orders.Queries.GetMyOrdersQuery;
using ReGreenShop.Application.Users.Commands;
using ReGreenShop.Application.Users.Queries.GetAllUnreadNotificationsQuery;
using ReGreenShop.Application.Users.Queries.GetUserInfo;
using ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;

namespace ReGreenShop.Web.Controllers;
public class UserController : BaseController
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(nameof(GetUserInfoForOrder))]
    public async Task<IActionResult> GetUserInfoForOrder()
    {
        var query = new GetUserInfoForOrderQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [Authorize]
    [HttpGet(nameof(GetUserInfo))]
    public async Task<IActionResult> GetUserInfo()
    {
        var query = new GetUserInfoQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [Authorize]
    [HttpGet(nameof(GetTotalGreenImpact))]
    public async Task<IActionResult> GetTotalGreenImpact()
    {
        var query = new TotalGreenImpactQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [Authorize]
    [HttpGet(nameof(GetAllUnReadNotifications))]
    public async Task<IActionResult> GetAllUnReadNotifications()
    {
        var query = new GetAllUnReadNotificationsQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    // commands
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
        return ApiResponseHelper.Created(result, "Registration successful! You are now logged in.");
    }

    [Authorize]
    [HttpGet(nameof(ReadNotifications))]
    public async Task<IActionResult> ReadNotifications()
    {
        var command = new ReadNotificationsCommand();
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result);
    }
}
