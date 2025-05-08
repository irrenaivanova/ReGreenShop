using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Products.Queries.GetProductById.Models;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Common.Exceptions;
using AutoMapper;

namespace ReGreenShop.Application.Products.Queries.GetProductById;
public class GetProductByIdQuery : IRequest<ProductByIdModel>
{
    public int Id { get; set; }

    public class GetProductByIdQueryValidator : IRequestHandler<GetProductByIdQuery, ProductByIdModel>
    {
        private readonly IData data;
        private readonly IMapper mapper;

        public GetProductByIdQueryValidator(IData data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<ProductByIdModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            //var productEntity = await this.data.Products
            //            .Where(x => x.Id == request.Id)
            //            .Include(x => x.ProductCategories)
            //            .ThenInclude(x => x.Category)
            //            .AsNoTracking()
            //            .FirstOrDefaultAsync();

            return null;


        }
    }
}
