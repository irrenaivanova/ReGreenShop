using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Categories.Queries.GetRootCategories;
public class RootCategoriesModel : IMapFrom<Category>, IMapExplicitly
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Category, RootCategoriesModel>()
            .ForMember(x => x.ImagePath, cfg => cfg.MapFrom(x => x.Image!.BlobPath ?? x.Image.LocalPath))
            .ForMember(x => x.Name, cfg => cfg.MapFrom(x => x.NameInEnglish));
    }
}
