using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Orders.Commands.MakeAnOrder;
using ReGreenShop.Application.Orders.Queries.GetMyOrdersQuery;


namespace ReGreenShop.Web.Controllers;
public class OrderController : BaseController
{
    private readonly IMediator mediator;

    public OrderController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Authorize]
    [HttpGet(nameof(GetMyOrders))]
    public async Task<IActionResult> GetMyOrders()
    {
        var query = new GetMyOrdersQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
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
