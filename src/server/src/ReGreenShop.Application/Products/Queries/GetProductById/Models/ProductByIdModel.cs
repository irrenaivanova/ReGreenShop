using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Products.Queries.GetProductById.Models;
public class ProductByIdModel : IMapFrom<Product>, IMapExplicitly
{
    public ProductByIdModel()
    {
        Categories = new List<CategoryModel>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Price { get; set; } = string.Empty;

    public int Stock { get; set; }

    public string? ProductCode { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public string? Packaging { get; set; }

    public string? OriginalUrl { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public List<CategoryModel> Categories { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Product, ProductByIdModel>()
            .ForMember(x => x.ImagePath, cfg => cfg.MapFrom(x => x.Image!.BlobPath ?? x.Image.LocalPath))
            .ForMember(x => x.Price, cfg => cfg.MapFrom(x => $"{x.Price} lv"))
            .ForMember(x => x.Categories, cfg => cfg.MapFrom(x => x.ProductCategories));
    }
}
