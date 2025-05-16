using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Domain.Entities.Enum;

namespace ReGreenShop.Application.Orders.Queries.Models;
public class GetOrderModel : IMapFrom<Order>, IMapExplicitly
{
    public string Id { get; set; } = string.Empty;

    public string CreatedOn { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string Payment { get; set; } = string.Empty;

    public string InvoiceUrl { get; set; } = string.Empty;

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Order, GetOrderModel>()
            .ForMember(x => x.Id, cfg => cfg.MapFrom(x => x.Id.Substring(0, 6)))
            .ForMember(x => x.CreatedOn, cfg => cfg.MapFrom(x => x.CreatedOn.ToString("dd MMM yyyy")))
            .ForMember(x => x.Status, cfg => cfg.MapFrom(x => x.Status.ToString()))
            .ForMember(x => x.Payment, cfg => cfg.MapFrom(x => x.Payment.PaymentMethod.Name));
    }
}
