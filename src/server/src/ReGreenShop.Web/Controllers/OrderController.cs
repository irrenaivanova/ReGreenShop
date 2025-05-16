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
    public async Task<IActionResult> MakeAnOrder([FromBody] MakeAnOrderModel model)
    {
        var command = new MakeAnOrderCommand(model);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result!, "Your order has been placed successfully. You will receive a confirmation email along with an invoice for your purchase.");
    }
}
