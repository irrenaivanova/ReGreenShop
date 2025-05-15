using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IDelivery : IService
{
    (decimal? deliveryCost, string deliveryMessage) CalculateTheDeliveryPrice(decimal totalPriceProducts);
}
