using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.AdminArea.Queries.GetAllProductsQuery;
public record SearchAllProducts(int page,
    int pageSize,
    string? searchString,
    decimal? minPrice,
    decimal? maxPrice,
    decimal? minStock,
    decimal? maxStock) : IRequest<AllProductsAdminPaginated>
{
    public class SearchAllProductsHandler : IRequestHandler<SearchAllProducts, AllProductsAdminPaginated>
    {
        private readonly IData data;

        public SearchAllProductsHandler(IData data)
        {
            this.data = data;
        }

        public async Task<AllProductsAdminPaginated> Handle(SearchAllProducts request, CancellationToken cancellationToken)
        {
            var query = this.data.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.searchString))
            {
                var words = request.searchString
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var word in words)
                {
                    var searchWord = word;
                    query = query.Where(p =>
                        (p.Name != null && p.Name.Contains(searchWord)) ||
                        p.ProductCategories.Any(pc =>
                            (pc.Category != null && (
                                (pc.Category.NameInEnglish != null && pc.Category.NameInEnglish.Contains(searchWord)) ||
                                (pc.Category.NameInBulgarian != null && pc.Category.NameInBulgarian.Contains(searchWord))
                            ))
                        ) ||
                        p.LabelProducts.Any(lp =>
                            lp.Label != null && lp.Label.Name != null && lp.Label.Name.Contains(searchWord)
                        )
                    );
                }
            }

            if (request.minPrice.HasValue)
            {
                query = query.Where(x => x.Price >= request.minPrice.Value);
            }

            if (request.maxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= request.maxPrice.Value);
            }

            if (request.minStock.HasValue)
            {
                query = query.Where(x => x.Stock >= request.minStock.Value);
            }

            if (request.maxStock.HasValue)
            {
                query = query.Where(x => x.Stock <= request.maxStock.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling(totalCount / (double)request.pageSize);

            var products = await query.OrderBy(p => p.Id)
                         .OrderBy(x => x.Stock)  
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
