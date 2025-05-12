using System.Globalization;
using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Application.Products.Models;
public class ProductInList2 : IMapFrom<Product>, IMapExplicitly
{
    public ProductInList2()
    {
        Labels = new List<string>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? Packaging { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public bool IsLiked { get; set; }

    public bool HasPromoDiscount { get; set; }

    public int DiscountPercentage { get; set; }

    public string? ValidTo { get; set; }

    public string? AdditionalTextForPromotion { get; set; }

    public bool HasTwoForOneDiscount { get; set; }

    public decimal DiscountPrice { get; set; }

    public List<string> Labels { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Product, ProductInList2>()
            .ForMember(x => x.ImagePath, cfg => cfg.MapFrom(x => x.Image!.BlobPath ?? x.Image.LocalPath))
            .ForMember(x => x.Labels, cfg => cfg.MapFrom(x => x.LabelProducts.Select(x => x.Label.Name)))
            .ForMember(x => x.HasTwoForOneDiscount, cfg => cfg.MapFrom(x => x.LabelProducts.Any(y => y.Label.Name == TwoForOne)));
         

    }
}
