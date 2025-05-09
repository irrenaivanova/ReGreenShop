using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReGreenShop.Application.Categories.Queries.GetRootCategories;
using ReGreenShop.Application.Categories.Queries.GetSubCategoriesByRootCategoryId;
using ReGreenShop.Application.Common.Helpers;

namespace ReGreenShop.Web.Controllers;
public class CategoryController : BaseController
{
    private readonly IMediator mediator;

    public CategoryController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet(nameof(GetRootCategories))]
    public async Task<IActionResult> GetRootCategories()
    {
        var query = new GetRootCategoriesQuery();
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }

    [HttpGet("GetSubCategoriesByRootCategory/{id}")]
    public async Task<IActionResult> GetSubCategoriesByRooCategoryId(int id)
    {
        var query = new GetSubCategoriesByRootCategoryIdQuery(id);
        var result = await this.mediator.Send(query);
        return ApiResponseHelper.Success(result);
    }
}
