using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        => Ok(await this.mediator.Send(command));

    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    => Ok(await this.mediator.Send(command));
}
