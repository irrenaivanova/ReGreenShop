using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Categories.Queries.GetSubCategoriesByRootCategoryId.Models;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Categories.Queries.GetSubCategoriesByRootCategoryId;
public record GetSubCategoriesByRootCategoryIdQuery(int id) : IRequest<RootCategoryModel>
{
    public class GetSubCategoriesByRootCategoryIdQueryHandler : IRequestHandler<GetSubCategoriesByRootCategoryIdQuery, RootCategoryModel>
    {
        private readonly IData data;

        public GetSubCategoriesByRootCategoryIdQueryHandler(IData data)
        {
            this.data = data;
        }

        public async Task<RootCategoryModel> Handle(GetSubCategoriesByRootCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var category = await this.data.Categories
                .Where(x => x.Id == request.id && x.ParentCategoryId == null)
                .Include(x => x.ChildCategories)
                .AsNoTracking()
                .Select(r => new RootCategoryModel
                {
                    Id = r.Id,
                    Name = r.NameInEnglish ?? r.NameInBulgarian ?? string.Empty,
                    SubCategories = r.ChildCategories.Select(x => new SubCategoryModel
                    {
                        Id = x.Id,
                        Name = x.NameInEnglish ?? x.NameInBulgarian ?? string.Empty,
                        SubSubCategories = x.ChildCategories.Select(y => new SubSubCategoryModel
                        {
                            Id = y.Id,
                            Name = y.NameInEnglish ?? y.NameInBulgarian ?? string.Empty,
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new NotFoundException("Root category", request.id);
            }

            return category;
        }
    }
}
