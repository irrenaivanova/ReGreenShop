using System.Globalization;
using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Products.Models;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Application.AdminArea.Queries.GetAllProductsQuery;
public class AdminProductInListModel : IMapFrom<Product>, IMapExplicitly
{
    public AdminProductInListModel()
    {
        Labels = new List<string>();
        Categories = new List<string>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? Packaging { get; set; }

    public string? ProductCode { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public int? DiscountPercentage { get; set; }

    public int Stock { get; set; }

    public List<string> Labels { get; set; }

    public List<string> Categories { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Product, AdminProductInListModel>()
        .ForMember(x => x.ImagePath, cfg => cfg.MapFrom(x => x.Image!.BlobPath ?? x.Image.LocalPath))
        .ForMember(x => x.Categories, cfg => cfg.MapFrom(x => x.ProductCategories.Select(x => x.Category.NameInEnglish)))
        .ForMember(x => x.Labels, cfg => cfg.MapFrom(x => x.LabelProducts.Select(x => x.Label.Name)))
        .ForMember(x => x.DiscountPercentage, cfg => cfg.MapFrom(x => x.LabelProducts.Any(y => y.Label.Name == Offer) ?
                                                        x.LabelProducts.FirstOrDefault(y => y.Label.Name == Offer)!.PercentageDiscount : null));
   }
}
