using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Services;

namespace ReGreenShop.Application.Products.Queries;
public record GetProductByIdQuery(int id) : IRequest<ProductByIdModel>
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductByIdModel>
    {
        private readonly IData data;
        private readonly ICurrentUser userService;

        public GetProductByIdQueryHandler(IData data, ICurrentUser userService)
        {
            this.data = data;
            this.userService = userService;
        }
        public async Task<ProductByIdModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var prod = await this.data.Products
                        .Where(x => x.Id == request.id)
                        .To<ProductByIdModel>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

            if (prod == null)
            {
                throw new NotFoundException("Product", request.id);
            }

            if (prod.HasPromoDiscount && !prod.HasTwoForOneDiscount && prod.DiscountPercentage.HasValue)
            {
                prod.DiscountPrice = PriceCalculator.CalculateDiscountedPrice(prod.Price, prod.DiscountPercentage.Value);
                prod.Labels.Add($"SAVE {prod.DiscountPercentage}%");
            }

            string? userId = this.userService.UserId;
            if (userId != null)
            {
                prod.IsLiked = this.data.Products.Where(x => x.Id == request.id).Any(x => x.UserLikes.Any(x => x.UserId == userId));
            }

            return prod;
        }
    }
}

