using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Labels.Queries;

namespace ReGreenShop.Web.Controllers;
public class LabelController : BaseController
{
    private readonly IMediator mediator;

    public LabelController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(nameof(GetAllLabels))]
    public async Task<IActionResult> GetAllLabels()
    {
        var query = new GetAllLabelsQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }
}
