using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Products.Queries.GetProductById.Models;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Common.Exceptions;
using AutoMapper;

namespace ReGreenShop.Application.Products.Queries.GetProductById;
public record GetProductByIdQuery(int Id) : IRequest<ProductByIdModel>
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductByIdModel>
    {
        private readonly IData data;
        private readonly IMapper mapper;

        public GetProductByIdQueryHandler(IData data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<ProductByIdModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productEntity = await this.data.Products
                        .Where(x => x.Id == request.Id)
                        .Include(x => x.ProductCategories)
                        .ThenInclude(x => x.Category)
                        .To<ProductByIdModel>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

            if (productEntity == null)
            {
                throw new NotFoundException("Product", request.Id);
            }

            return productEntity;
        }
    }
}
