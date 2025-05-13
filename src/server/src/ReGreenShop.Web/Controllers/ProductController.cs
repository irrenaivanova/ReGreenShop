using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Products.Commands;
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

    [HttpGet(nameof(ProductsByLabel) + "/{id}")]
    public async Task<IActionResult> ProductsByLabel(int id)
    {
        var query = new GetProductsByLabelQuery(id);
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(ProductsBySubCategory) + "/{id}")]
    public async Task<IActionResult> ProductsBySubCategory(int id)
    {
        var query = new GetProductsBySubCategory(id);
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(ProductsByRootCategory))]
    public async Task<IActionResult> ProductsByRootCategory([FromQuery] GetProductsByRootCategory query)
    {
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(ProductsBySearchString))]
    public async Task<IActionResult> ProductsBySearchString([FromQuery] SearchByStringQuery query)
    {
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }


    [Authorize]
    [HttpGet(nameof(GetMyProducts))]
    public async Task<IActionResult> GetMyProducts()
    {
        var query = new GetMyProductsQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }


    // Commands
    [Authorize]
    [HttpGet(nameof(LikeAProduct) + "/{id}")]
    public async Task<IActionResult> LikeAProduct(int id)
    {
        var command = new LikeProductCommand(id);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result);
    }
}
