using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Services;

namespace ReGreenShop.Application.Products.Queries;
public record GetMyProductsQuery() : IRequest<IEnumerable<ProductInList>>
{
    public class GetTopProductsQueryHandler : IRequestHandler<GetMyProductsQuery, IEnumerable<ProductInList>>
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

        public async Task<IEnumerable<ProductInList>> Handle(GetMyProductsQuery request, CancellationToken cancellationToken)
        {
            string? userId = this.userService.UserId;
            if (userId == null)
            {
                throw new NotFoundException("User", "null");
            }
            var myProducts = new List<ProductInList>();
            var likedProducts = await this.data.Products
                .Where(x => x.UserLikes.Any(x => x.UserId == userId))
                .To<ProductInList>()
                .AsNoTracking()
                .ToListAsync();
            myProducts.AddRange(likedProducts);

            var lastOrder = await this.data.Orders.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (lastOrder != null)
            {
                var lastOrderId = lastOrder.Id;
                var productFromLastOrder = await this.data.Products
                            .Where(x => x.OrderDetails.Any(x => x.OrderId == lastOrderId))
                            .To<ProductInList>().ToListAsync();
                myProducts.AddRange(productFromLastOrder);
            }


            foreach (var prod in myProducts)
            {
                if (prod.HasPromoDiscount && !prod.HasTwoForOneDiscount && prod.DiscountPercentage.HasValue)
                {
                    prod.DiscountPrice = PriceCalculator.CalculateDiscountedPrice(prod.Price, prod.DiscountPercentage.Value);
                    prod.Labels.Add($"SAVE {prod.DiscountPercentage}%");
                }

                if (userId != null)
                {
                    prod.IsLiked = this.data.Products.Where(x => x.Id == prod.Id).Any(x => x.UserLikes.Any(x => x.UserId == userId));
                }

                prod.ProductCartQuantity = await this.cartService.GetCountOfConcreteProductInCartAsync(prod.Id);
            }

            return myProducts;
        }
    }
}
