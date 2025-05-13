using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Carts.Commands;
using ReGreenShop.Application.Carts.Queries;
using ReGreenShop.Application.Common.Helpers;

namespace ReGreenShop.Web.Controllers;
public class CartController : BaseController
{
    private readonly IMediator mediator;

    public CartController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(nameof(NumberOfProductsInCart))]
    public async Task<IActionResult> NumberOfProductsInCart()
    {
        var query = new GetNumberOfProductsInCart();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    // commands
    [HttpGet(nameof(AddToCart) + "/{id}")]
    public async Task<IActionResult> AddToCart(int id)
    {
        var command = new AddToCartCommand(id);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(RemoveFromCart) + "/{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var command = new RemoveFromCartCommand(id);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(CleanCart))]
    public async Task<IActionResult> CleanCart()
    {
        var command = new CleanCartCommand();
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result);
    }
}
