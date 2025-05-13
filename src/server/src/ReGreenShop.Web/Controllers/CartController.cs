using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Carts.Commands;
using ReGreenShop.Application.Common.Helpers;
using ReGreenShop.Application.Products.Commands;

namespace ReGreenShop.Web.Controllers;
public class CartController : BaseController
{
    private readonly IMediator mediator;

    public CartController(IMediator mediator)
    {
        this.mediator = mediator;
    }

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
}
