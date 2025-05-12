using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;

namespace ReGreenShop.Application.Products.Queries;
public class SearchByStringQuery : IRequest<AllProductsPaginated>
{
    public string SearchString { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int ItemsPerPage { get; set; } = Common.GlobalConstants.ItemsPerPage;

    public class SearchByStringQueryHandler : IRequestHandler<SearchByStringQuery, AllProductsPaginated>
    {
        private readonly IData data;

        public SearchByStringQueryHandler(IData data)
        {
            this.data = data;
        }

        public AllProductsPaginated Handle(SearchByStringQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchString))
            {
                return null!;
            }

            var wildCarts = request.SearchString.ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(x => $"%{x.ToLower()}%").ToList();

            var query = this.data.Products
                   .Select(product => new ProductScoreModel
                   {
                       Product = product,
                       Score = wildCarts.Count(search =>
                           EF.Functions.Like(product.Name, search) ||
                           EF.Functions.Like(product.Brand, search) ||
                           EF.Functions.Like(product.Origin, search) ||
                           EF.Functions.Like(product.Packaging, search)
                       )
                   });

            foreach (var item in query)
            {
                var categoryScore = wildCarts.Count(search =>
                    item.Product.ProductCategories.Any(x => EF.Functions.Like(x.Category.NameInBulgarian, search)) ||
                    item.Product.ProductCategories.Any(x => EF.Functions.Like(x.Category.NameInEnglish, search))
                );

                item.Score += categoryScore;
            }

            var products = query
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Skip((request.Page - 1) * request.ItemsPerPage)
                .Take(request.ItemsPerPage)
                .Select(x => x.Product)
                .To<ProductInList>()
                .ToList();

            var totalItems = query.Where(x => x.Score > 0).Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.ItemsPerPage);
            if (request.Page < 1 || request.Page > totalPages)
            {
                throw new NotFoundException("Page", request.Page);
            }

            var productsPaginated = new AllProductsPaginated()
            {
                Products = products,
                PageSize = request.ItemsPerPage,
                CurrentPage = request.Page,
                TotalPages = totalPages,
                TotalItems = totalItems,
            };

            return productsPaginated;
        }

        Task<AllProductsPaginated> IRequestHandler<SearchByStringQuery, AllProductsPaginated>.Handle(SearchByStringQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
