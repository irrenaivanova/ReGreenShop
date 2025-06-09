using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Common.Identity.Login;
using ReGreenShop.Application.Common.Identity.Register;
using ReGreenShop.Application.GreenAlternatives.GetTotalGreenImpact;
using ReGreenShop.Application.Orders.Queries.GetMyOrdersQuery;
using ReGreenShop.Application.Users.Commands;
using ReGreenShop.Application.Users.Queries.GetAllUnreadNotificationsQuery;
using ReGreenShop.Application.Users.Queries.GetUserInfo;
using ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;
using ReGreenShop.Infrastructure.Persistence.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReGreenShop.Web.Controllers;
public class UserController : BaseController
{
    private readonly IMediator mediator;
    private readonly SignInManager<User> signInManager;
    private readonly LinkGenerator linkGenerator;
    private readonly IHttpContextAccessor context;

    public UserController(IMediator mediator, SignInManager<User> signInManager,
                           LinkGenerator linkGenerator, IHttpContextAccessor context)
    {
        this.mediator = mediator;
        this.signInManager = signInManager;
        this.linkGenerator = linkGenerator;
        this.context = context;
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

    [HttpGet("login/google")]
    public IActionResult LoginWithGoogle(string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = "/";
        }
        var redirectUrl = this.linkGenerator.GetPathByName(this.context.HttpContext!, nameof(GoogleLoginCallback))
                       + $"?returnUrl={Uri.EscapeDataString(returnUrl)}";

        var properties = this.signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("login/google/callback")]
    public async Task<IActionResult> GoogleLoginCallback(string returnUrl)
    {
        var command = new HandleGoogleLoginCommand(returnUrl);
        var result = await this.mediator.Send(command);
        var queryParams = new Dictionary<string, string>
    {
        { "accessToken", result.AccessToken },
        { "userId", result.UserId },
        { "userName", result.UserName },
        { "isAdmin", result.IsAdmin.ToString().ToLower() } 
    };

        var redirectUrl = QueryHelpers.AddQueryString(returnUrl, queryParams);
        return Redirect(redirectUrl);
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
