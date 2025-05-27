using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery.Models;
public class ProductInCartModel : IMapFrom<Product>, IMapExplicitly
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? Packaging { get; set; }

    public string? LabelTwoForOne { get; set; }

    public string ImagePath { get; set; } = string.Empty;

    public bool HasPromoDiscount { get; set; }

    public bool HasTwoForOneDiscount { get; set; }

    public int? DiscountPercentage { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int QuantityInCart { get; set; }

    public int Stock { get; set; }

    public decimal TotalPriceProduct { get; set; }


    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Product, ProductInCartModel>()
            .ForMember(x => x.ImagePath, cfg => cfg.MapFrom(x => x.Image!.BlobPath ?? x.Image.LocalPath))
            .ForMember(x => x.HasPromoDiscount, cfg => cfg.MapFrom(x => x.LabelProducts.Any(y => y.Label.Name == Offer)))
            .ForMember(x => x.HasTwoForOneDiscount, cfg => cfg.MapFrom(x => x.LabelProducts.Any(y => y.Label.Name == TwoForOne)))
            .ForMember(x => x.DiscountPercentage, cfg => cfg.MapFrom(x => x.LabelProducts.Any(y => y.Label.Name == Offer) ?
                                                            x.LabelProducts.FirstOrDefault(y => y.Label.Name == Offer)!.PercentageDiscount : 0));
    }
}
