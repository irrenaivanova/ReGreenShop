using MediatR;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.AdminArea.Queries.GetAllProductsQuery;
public record GetAllProductsQuery(int page,
    int pageSize,
    string? name,
    string? category,
    string? label,
    decimal? minPrice,
    decimal? maxPrice) : IRequest<IEnumerable<AdminProductInListModel>>
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<AdminProductInListModel>>
    {
        private readonly IData data;

        public GetAllProductsQueryHandler(IData data)
        {
            this.data = data;
        }

        public Task<IEnumerable<AdminProductInListModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var query = this.data.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.category))
            {
                query = query.Where(x =>
                    x.ProductCategories.Any(x => x.Category.NameInEnglish == request.category.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.label))
            {
                query = query.Where(x =>
                    x.LabelProducts.Any(x => x.Label.Name.ToLower() == request.label.ToLower()));
            }

            if (request.minPrice.HasValue)
            {
                query = query.Where(x => x.Price >= request.minPrice.Value);
            }

            if (request.maxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= request.maxPrice.Value);
            }

            query = query.OrderBy(p => p.Id)
                         .Skip((request.page - 1) * request.pageSize)
                         .Take(request.pageSize);

            var result = query.To<AdminProductInListModel>().ToList();

            return Task.FromResult(result.AsEnumerable());

        }
    }
}
