using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Products.Queries;
public record  GetProductsByLabelQuery(int id) : IRequest<IEnumerable<ProductInList>>
{
    public class GetProductsByLabelHandler : IRequestHandler<GetProductsByLabelQuery, IEnumerable<ProductInList>>
    {
        private readonly IData data;

        public GetProductsByLabelHandler(IData data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<ProductInList>> Handle(GetProductsByLabelQuery request, CancellationToken cancellationToken)
        {
            var products = await this.data.Products
                .Where(x => x.LabelProducts.Any(x => x.LabelId == request.id))
                .To<ProductInList>()
                .AsNoTracking()
                .ToListAsync();

            if (!products.Any())
            {
                throw new NotFoundException("Label", request.id);
            }
            return products;
        }
    }
}
