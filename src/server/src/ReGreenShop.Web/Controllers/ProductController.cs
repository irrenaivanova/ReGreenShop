using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Products.Queries;

namespace ReGreenShop.Web.Controllers;
public class ProductController : BaseController
{
    private readonly IMediator mediator;

    public ProductController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(GetTopProducts))]
    public async Task<IActionResult> GetTopProducts()
    {
        var query = new GetTopProductsQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }
}
