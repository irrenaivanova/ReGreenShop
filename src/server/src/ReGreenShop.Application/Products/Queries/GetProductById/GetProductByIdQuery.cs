using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Queries.GetProductById.Models;

namespace ReGreenShop.Application.Products.Queries.GetProductById;
public record GetProductByIdQuery(int id) : IRequest<ProductByIdModel>
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductByIdModel>
    {
        private readonly IData data;
        public GetProductByIdQueryHandler(IData data)
        {
            this.data = data;

        }

        public async Task<ProductByIdModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productEntity = await this.data.Products
                        .Where(x => x.Id == request.id)
                        .Include(x => x.ProductCategories)
                        .ThenInclude(x => x.Category)
                        .To<ProductByIdModel>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

            if (productEntity == null)
            {
                throw new NotFoundException("Product", request.id);
            }

            return productEntity;
        }
    }
}
