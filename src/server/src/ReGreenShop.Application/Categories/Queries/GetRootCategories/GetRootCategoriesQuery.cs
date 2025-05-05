using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

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
                .To<RootCategoriesModel>()
                .ToListAsync();

            if (categories.Count == 0)
            {
                throw new NotFoundException("Root categories", "all");
            }

            return categories;
        }
    }
}
