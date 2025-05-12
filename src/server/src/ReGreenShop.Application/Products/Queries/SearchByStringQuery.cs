using System.Linq;
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

        public async Task<AllProductsPaginated> Handle(SearchByStringQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchString))
            {
                return null!;
            }

            var searchs = request.SearchString.ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();

            var query = this.data.Products.Include(x => x.LabelProducts).ThenInclude(x => x.Label)
                   .Select(product => new ProductScoreModel
                   {
                       Product = product,
                       Score = searchs.Count(search =>
                           product.Name.Contains(search) ||
                           product.Brand != null ? product.Brand!.Contains(search) : false ||
                           product.Origin != null ? product.Origin!.Contains(search) : false ||
                           product.Packaging != null ? product.Packaging!.Contains(search) : false
                       )
                   });

            var categoryIds = this.data.Categories
                 .Where(cat => searchs.Any(search =>
                     cat.NameInBulgarian!.ToLower().Contains(search) ||
                     cat.NameInEnglish!.ToLower().Contains(search)))
                 .Select(cat => cat.Id);

            var categoryQuery = this.data.Products.Include(x => x.LabelProducts).ThenInclude(x => x.Label)
                .Where(p => p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId)))
                .Select(product => new ProductScoreModel
                {
                    Product = product,
                    Score = 1
                });

            var combinedQuery = query.Concat(categoryQuery);

            //if (item.Product.ProductCategories.Any())
            //{
            //    var categoryScore = searchs.Count(search =>
            //    item.Product.ProductCategories.Any(x => x.Category.NameInBulgarian!.Contains(search) ||
            //    item.Product.ProductCategories.Any(x => x.Category.NameInEnglish!.Contains(search))));

            //    item.Score += categoryScore;
            //}


            var products = await combinedQuery
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Skip((request.Page - 1) * request.ItemsPerPage)
                .Take(request.ItemsPerPage)
                .Select(x => x.Product)
                .To<ProductInList>()
                .ToListAsync();

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
    }
}
