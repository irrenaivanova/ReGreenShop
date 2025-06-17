using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Cities.Queries;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Deliveries.Queries;
using ReGreenShop.Application.GreenAlternatives.GetAllGreenAlternativesQuery;
using ReGreenShop.Application.Labels.Queries;
using ReGreenShop.Application.Payments.Queries;
using ReGreenShop.Application.Vouchers.Queries;

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

    [HttpGet(nameof(GetAllPaymentMethods))]
    public async Task<IActionResult> GetAllPaymentMethods()
    {
        var query = new GetAllPaymentMethodQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }


    [HttpGet(nameof(GetAllVouchers))]
    public async Task<IActionResult> GetAllVouchers()
    {
        var query = new GetAllVouchersQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }


    [HttpGet(nameof(GetAllCities))]
    public async Task<IActionResult> GetAllCities()
    {
        var query = new GetAllCitiesQuery();
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

    [HttpGet(nameof(GetAllGreenAlternatives))]
    public async Task<IActionResult> GetAllGreenAlternatives()
    {
        var query = new GetAllGreenAlternativesQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpPost(nameof(ResetSession))]
    public IActionResult ResetSession()
    {
        var context = HttpContext;
        context.Session.Clear();
        context.Response.Cookies.Delete(".AspNetCore.Session");
        return Ok(new { message = "Session has been reset." });
    }
}
