using MediatR;
using Microsoft.EntityFrameworkCore;
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
                .Where(x => x.ParentCategoryId == null)
                .AsNoTracking()
                .To<RootCategoriesModel>()
                .ToListAsync();


            return categories;
        }
    }
}
