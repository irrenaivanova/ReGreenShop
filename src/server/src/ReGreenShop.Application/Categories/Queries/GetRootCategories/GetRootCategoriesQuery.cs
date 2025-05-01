using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Categories.Queries.GetRootCategories;
public class GetRootCategoriesQuery : IRequest<IEnumerable<RootCategoriesModel>>
{
    public class GetRootCategoriesQueryHandler : IRequestHandler<GetRootCategoriesQuery, IEnumerable<RootCategoriesModel>>
    {
        private readonly IData data;

        public GetRootCategoriesQueryHandler(IData data)
        {
            this.data = data;
        }
        public async Task<IEnumerable<RootCategoriesModel>> Handle(GetRootCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await this.data.Categories
                .Include(x => x.Image)
                .Where(x => x.ParentCategoryId == null)
                .AsNoTracking()
                .Select(x => new RootCategoriesModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImagePath = x.Image != null
                         ? (x.Image.BlobPath ?? x.Image.LocalPath ?? string.Empty)
                         : string.Empty 
                }).ToListAsync();

            if (categories.Count == 0)
            {
                throw new FileNotFoundException("No root categories found!");
            }

            return categories;
        }
    }
}
