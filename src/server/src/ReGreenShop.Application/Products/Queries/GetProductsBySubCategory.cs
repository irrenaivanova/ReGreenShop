using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Services;

namespace ReGreenShop.Application.Products.Queries;
public record  GetProductsBySubCategory(int id) : IRequest<IEnumerable<ProductInList>>
{
    public class GetProductBySubCategoryHandler : IRequestHandler<GetProductsBySubCategory, IEnumerable<ProductInList>>
    {
        private readonly IData data;
        private readonly ICurrentUser userService;
        private readonly ICart cartService;

        public GetProductBySubCategoryHandler(IData data, ICurrentUser userService, ICart cartService)
        {
            this.data = data;
            this.userService = userService;
            this.cartService = cartService;
        }

        public async Task<IEnumerable<ProductInList>> Handle(GetProductsBySubCategory request, CancellationToken cancellationToken)
        {
            var category = await this.data.Categories.FirstOrDefaultAsync(x => x.Id == request.id);
            if (category == null || category.ParentCategoryId == null)
            {
                throw new NotFoundException("SubCategory", request.id);
            }

            var products = await this.data.Products
                .Where(x => x.ProductCategories.Any(x => x.CategoryId == category.Id))
                .OrderByDescending(x => x.Stock)
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

            return products;
        }
    }
}
