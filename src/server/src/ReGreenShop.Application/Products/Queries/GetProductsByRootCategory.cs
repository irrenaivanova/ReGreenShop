using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Services;

namespace ReGreenShop.Application.Products.Queries;
public class GetProductsByRootCategory() : IRequest<AllProductsPaginated>
{
    public int CategoryId { get; set; }
    public int Page { get; set; } = 1;
    public int ItemsPerPage { get; set; } = Common.GlobalConstants.ItemsPerPage;

    public class GetProductsByRootCategoryHandler : IRequestHandler<GetProductsByRootCategory, AllProductsPaginated>
    {
        private readonly IData data;
        private readonly ICurrentUser userService;
        private readonly ICart cartService;

        public GetProductsByRootCategoryHandler(IData data, ICurrentUser userService, ICart cartService)
        {
            this.data = data;
            this.userService = userService;
            this.cartService = cartService;
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


            foreach (var prod in products)
            {
                if (prod.HasPromoDiscount && !prod.HasTwoForOneDiscount && prod.DiscountPercentage.HasValue)
                {
                    prod.DiscountPrice = PriceCalculator.CalculateDiscountedPrice(prod.Price, prod.DiscountPercentage.Value);
                    prod.Labels.Add($"SAVE {prod.DiscountPercentage}%");
                }

                string? userId = this.userService.UserId;
                if (userId != null)
                {
                    prod.IsLiked = this.data.Products.Where(x => x.Id == prod.Id).Any(x => x.UserLikes.Any(x => x.UserId == userId));
                }

                prod.ProductCartQuantity = await this.cartService.GetCountOfConcreteProductInCartAsync(prod.Id);
            }

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
