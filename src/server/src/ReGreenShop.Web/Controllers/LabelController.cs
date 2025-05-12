using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Labels.Queries;
using ReGreenShop.Application.Products.Queries;
using MediatR;

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
