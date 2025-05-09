using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Services;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Application.Products.Queries;
public record GetMyProductsQuery() : IRequest<IEnumerable<ProductInList>>
{
    public class GetTopProductsQueryHandler : IRequestHandler<GetMyProductsQuery, IEnumerable<ProductInList>>
    {
        private readonly IData data;
        private readonly ICurrentUser userService;

        public GetTopProductsQueryHandler(IData data, ICurrentUser userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public async Task<IEnumerable<ProductInList>> Handle(GetMyProductsQuery request, CancellationToken cancellationToken)
        {
            string? userId = this.userService.UserId;
            if (userId == null)
            {
                throw new NotFoundException("User","null");
            }
            var myProducts = new List<ProductInList>();
            var likedProducts = await this.data.Products
                .Where(x => x.UserLikes.Any(x => x.UserId == userId))
                .To<ProductInList>()
                .AsNoTracking()
                .ToListAsync();

            var lastOrderId = await this.data.Orders.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();


            // to take the products from the last order

            //myProducts.AddRange(likedProducts);
            //myProducts.AddRange(productFromLastOrder);

            foreach (var prod in myProducts)
            {
                if (prod.HasPromoDiscount && !prod.HasTwoForOneDiscount)
                {
                    prod.DiscountPrice = PriceCalculator.CalculateDiscountedPrice(prod.Price, prod.DiscountPercentage);
                    prod.Labels.Add($"SAVE {prod.DiscountPercentage}%");
                }

                if (userId != null)
                {
                    prod.IsLiked = this.data.Products.Where(x => x.Id == prod.Id).Any(x => x.UserLikes.Any(x => x.UserId == userId));
                }
            }

            return myProducts;
        }
    }
}
