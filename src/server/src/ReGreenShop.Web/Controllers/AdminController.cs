using System.Security.Permissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.AdminArea.Commands.FinishAnOrder;
using ReGreenShop.Application.AdminArea.Queries.GetAllProductsQuery;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Orders.Queries.GetPendingOrdersQuery;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Web.Controllers;

//[Authorize(Roles = AdminName)]
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
                     int pageSize = 10)
    {
        var query = new GetAllProducts(page, pageSize);
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpPost(nameof(SearchAll))]
    public async Task<IActionResult> SearchAll(
                 int page = 1,
                 int pageSize = 10,
                 string? searchString = null,
                 decimal? minPrice = null,
                 decimal? maxPrice = null,
                 decimal? minStock = null,
                 decimal? maxStock = null)
    {
        var query = new SearchAllProducts(page, pageSize, searchString, minPrice, maxPrice, minStock, maxStock);
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
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
