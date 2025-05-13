using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Deliveries.Queries;
using ReGreenShop.Application.Labels.Queries;

namespace ReGreenShop.Web.Controllers;
public class UtilityController : BaseController
{
    private readonly IMediator mediator;

    public UtilityController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(nameof(GetAllDeliveryPrices))]
    public async Task<IActionResult> GetAllDeliveryPrices()
    {
        var query = new GetDeliveryPricesQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(GetAllLabels))]
    public async Task<IActionResult> GetAllLabels()
    {
        var query = new GetAllLabelsQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }
}
