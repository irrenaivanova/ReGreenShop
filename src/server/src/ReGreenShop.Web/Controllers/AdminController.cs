using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.AdminArea.Commands.FinishAnOrder;
using ReGreenShop.Application.AdminArea.Queries.GetAllProductsQuery;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Orders.Queries.GetPendingOrdersQuery;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Web.Controllers;

[Authorize(Roles = AdminName)]
public class AdminController : BaseController
{
    private readonly IMediator mediator;

    public AdminController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    // Queries
    [HttpGet(nameof(GetPendingOrders))]
    public async Task<IActionResult> GetPendingOrders()
    {
        var query = new GetPendingOrdersQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll(
                     int page = 1,
                     int pageSize = 10,
                     string? name = null,
                     string? category = null,
                     string? label = null,
                     decimal? minPrice = null,
                     decimal? maxPrice = null)
    {
        var result = await this.mediator.Send(new GetAllProductsQuery(page, pageSize, name, category, label, minPrice, maxPrice));
        return Ok(result);
    }

    // Commands
    [HttpPost(nameof(FinishAnOrder))]
    public async Task<IActionResult> FinishAnOrder([FromBody] FinishAnOrderModel model)
    {
        var command = new FinishAnOrderCommand(model);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result!, "The order has been finished successfully.");
    }
}
