using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Products.Models;
public class CategoryModel : IMapFrom<ProductCategory>, IMapExplicitly
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<ProductCategory, CategoryModel>()
            .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.CategoryId))
            .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.Category.NameInEnglish ?? x.Category.NameInBulgarian));
    }
}
