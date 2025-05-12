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

namespace ReGreenShop.Application.Products.Queries;
public record  GetProductsBySubCategory(int id) : IRequest<IEnumerable<ProductInList>>
{
    public class GetProductBySubCategoryHandler : IRequestHandler<GetProductsBySubCategory, IEnumerable<ProductInList>>
    {
        private readonly IData data;

        public GetProductBySubCategoryHandler(IData data)
        {
            this.data = data;
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
            return products;
        }
    }
}
