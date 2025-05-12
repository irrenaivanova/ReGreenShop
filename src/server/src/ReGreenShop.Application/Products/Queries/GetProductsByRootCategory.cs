using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;

namespace ReGreenShop.Application.Products.Queries;
public class GetProductsByRootCategory() : IRequest<AllProductsPaginated>
{
    public int CategoryId { get; set; }
    public int Page { get; set; } = 1;
    public int ItemsPerPage { get; set; } = Common.GlobalConstants.ItemsPerPage;

    public class GetProductsByRootCategoryHandler : IRequestHandler<GetProductsByRootCategory, AllProductsPaginated>
    {
        private readonly IData data;

        public GetProductsByRootCategoryHandler(IData data)
        {
            this.data = data;
        }

        public async Task<AllProductsPaginated> Handle(GetProductsByRootCategory request, CancellationToken cancellationToken)
        {
            var category = await this.data.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId);
            if (category == null || category.ParentCategoryId != null)
            {
                throw new NotFoundException("Root Category", request.CategoryId);
            }

            var query = this.data.Products.AsNoTracking().Where(x => x.ProductCategories.Any(x => x.CategoryId == category.Id));
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.ItemsPerPage);

            if (request.Page < 1 || request.Page > totalPages)
            {
                throw new NotFoundException("Page", request.Page);
            }

            var products = await query
                .OrderByDescending(x => x.Stock)
                .Skip((request.Page - 1) * request.ItemsPerPage)
                .Take(request.ItemsPerPage)
                .To<ProductInList>().ToListAsync();

            var productsPaginated = new AllProductsPaginated()
            {
                Products = products,
                PageSize = request.ItemsPerPage,
                CurrentPage = request.Page,
                TotalPages = totalPages,
                TotalItems = totalItems,
            };

            return productsPaginated;
        }
    }
}
