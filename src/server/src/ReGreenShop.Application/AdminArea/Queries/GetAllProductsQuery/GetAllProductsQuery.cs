using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.AdminArea.Queries.GetAllProductsQuery;
public record GetAllProducts(int page,
    int pageSize) : IRequest<AllProductsAdminPaginated>
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProducts, AllProductsAdminPaginated>
    {
        private readonly IData data;

        public GetAllProductsHandler(IData data)
        {
            this.data = data;
        }

        public async Task<AllProductsAdminPaginated> Handle(GetAllProducts request, CancellationToken cancellationToken)
        {
            var query = this.data.Products.Include(x => x.Image).AsNoTracking().AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling(totalCount / (double)request.pageSize);

            var products = await query.OrderBy(p => p.Id)
                         .Skip((request.page - 1) * request.pageSize)
                         .Take(request.pageSize)
                         .To<AdminProductInListModel>()
                         .ToListAsync();

            var productsPaginated = new AllProductsAdminPaginated();
            productsPaginated.TotalPages = totalPages;
            productsPaginated.Products = products;

            return productsPaginated;
        }
    }
}
