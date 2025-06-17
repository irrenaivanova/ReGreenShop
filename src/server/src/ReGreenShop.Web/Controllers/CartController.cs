using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Carts.Commands;
using ReGreenShop.Application.Carts.Models;
using ReGreenShop.Application.Carts.Queries.GetNumberOfProductInCart;
using ReGreenShop.Application.Carts.Queries.GetNumberOfProductsInCart;
using ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery;
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

    [HttpGet(nameof(NumberOfConcreteProductInCart) + "/{id}")]
    public async Task<IActionResult> NumberOfConcreteProductInCart(int id)
    {
        var query = new GetNumberOfConcreteProductInCart(id);
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet(nameof(ViewProductsInCart))]
    public async Task<IActionResult> ViewProductsInCart()
    {
        var query = new ViewProductsInCartQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    // commands
    [HttpGet(nameof(AddToCart) + "/{id}")]
    public async Task<IActionResult> AddToCart(int id)
    {
        var command = new AddToCartCommand(id);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result, "Successfully added to cart");
    }

    [HttpGet(nameof(RemoveFromCart) + "/{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var command = new RemoveFromCartCommand(id);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result, "Successfully removed from cart");
    }

    [HttpGet(nameof(RemoveAllProducts) + "/{id}")]
    public async Task<IActionResult> RemoveAllProducts(int id)
    {
        var command = new RemoveAllOfAProductCommand(id);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result, "Successfully removed from cart");
    }

    [HttpGet(nameof(CleanCart))]
    public async Task<IActionResult> CleanCart()
    {
        var command = new CleanCartCommand();
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result);
    }

    [HttpPost(nameof(CreateStripeSession))]
    public async Task<IActionResult> CreateStripeSession([FromBody] CreateStripeSessionDto dto)
    {
        var command = new CreateStripeSession(dto.OrderId);
        var result = await this.mediator.Send(command);
        return ApiResponseHelper.Success(result);
    }
}
