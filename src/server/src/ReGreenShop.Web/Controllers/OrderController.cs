using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Common.Identity.Login;
using ReGreenShop.Application.Orders.Commands.MakeAnOrder;

namespace ReGreenShop.Web.Controllers;
public class OrderController : BaseController
{
    private readonly IMediator mediator;

    public OrderController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    // commands
    [HttpPost(nameof(MakeAnOrder))]
    public async Task<IActionResult> MakeAnOrder([FromBody] MakeAnOrderModel command)
    {
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result!, "Login successful");
    }
}
