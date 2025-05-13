using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Deliveries.Queries;
public class DeliveryPriceModel : IMapFrom<DeliveryPrice>, IMapExplicitly
{
    public string MinPriceOrder { get; set; } = string.Empty;

    public string MaxPriceOrder { get; set; } = string.Empty;

    public string Price { get; set; } = string.Empty;

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<DeliveryPrice, DeliveryPriceModel>()
            .ForMember(x => x.MinPriceOrder, cfg => cfg.MapFrom(x => $"{x.MinPriceOrder:F2} lv"))
            .ForMember(x => x.MaxPriceOrder, cfg => cfg.MapFrom(x => $"{x.MaxPriceOrder:F2} lv"))
            .ForMember(x => x.Price, cfg => cfg.MapFrom(x => $"{x.Price:F2} lv"));
    }
}
