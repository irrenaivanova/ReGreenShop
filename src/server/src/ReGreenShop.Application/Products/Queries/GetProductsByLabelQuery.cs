using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Services;

namespace ReGreenShop.Application.Products.Queries;
public record GetProductsByLabelQuery(int id) : IRequest<IEnumerable<ProductInList>>
{
    public class GetProductsByLabelHandler : IRequestHandler<GetProductsByLabelQuery, IEnumerable<ProductInList>>
    {
        private readonly IData data;
        private readonly ICurrentUser userService;
        private readonly ICart cartService;

        public GetProductsByLabelHandler(IData data, ICurrentUser userService, ICart cartService)
        {
            this.data = data;
            this.userService = userService;
            this.cartService = cartService;
        }

        public async Task<IEnumerable<ProductInList>> Handle(GetProductsByLabelQuery request, CancellationToken cancellationToken)
        {
            var products = await this.data.Products
                .Where(x => x.LabelProducts.Any(x => x.LabelId == request.id))
                .To<ProductInList>()
                .AsNoTracking()
                .ToListAsync();

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

            if (!products.Any())
            {
                throw new NotFoundException("Label", request.id);
            }
            return products;
        }
    }
}
