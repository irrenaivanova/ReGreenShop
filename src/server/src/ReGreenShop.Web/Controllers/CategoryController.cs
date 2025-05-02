using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Categories.Queries.GetRootCategories;

namespace ReGreenShop.Web.Controllers;
public class CategoryController : BaseController
{
    private readonly IMediator mediator;

    public CategoryController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(nameof(GetRootCategories))]
    public async Task<IActionResult> GetRootCategories() =>
         Ok(await this.mediator.Send(new GetRootCategoriesQuery()));

}
