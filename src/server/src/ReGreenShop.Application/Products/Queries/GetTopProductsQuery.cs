using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Services;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Application.Products.Queries;
public class GetTopProductsQuery : IRequest<IEnumerable<ProductInList>>
{
    public class GetTopProductsQueryHandler : IRequestHandler<GetTopProductsQuery, IEnumerable<ProductInList>>
    {
        private readonly IData data;
        private readonly ICurrentUser userService;
        private readonly ICart cartService;

        public GetTopProductsQueryHandler(IData data, ICurrentUser userService, ICart cartService)
        {
            this.data = data;
            this.userService = userService;
            this.cartService = cartService;
        }

        public async Task<IEnumerable<ProductInList>> Handle(GetTopProductsQuery request, CancellationToken cancellationToken)
        {
            var topProducts = new List<ProductInList>();
            var productsTwoForOne = await this.data.Products
                .Where(x => x.LabelProducts.Any(x => x.Label.Name == TwoForOne))
                .OrderByDescending(x => x.Stock)
                .To<ProductInList>()
                .AsNoTracking()
                .Take(ProductsInRow)
                .ToListAsync();

            var productsPromo = await this.data.Products
                .Where(x => x.LabelProducts.Any(x => x.Label.Name == Offer))
                .OrderByDescending(x => x.LabelProducts.FirstOrDefault(x => x.Label.Name == Offer)!.PercentageDiscount)
                .OrderBy(x => x.Stock)
                .To<ProductInList>()
                .AsNoTracking()
                .Take(ProductsInRow * 2)
                .ToListAsync();

            topProducts.AddRange(productsTwoForOne);
            topProducts.AddRange(productsPromo);

            foreach (var prod in topProducts)
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

            return topProducts;
        }
    }
}
