using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Infrastructure.Services;
public class DeliveryService : IDelivery
{
    private readonly IData data;

    public DeliveryService(IData data)
    {
        this.data = data;
    }
    public (decimal? deliveryCost, string deliveryMessage) CalculateTheDeliveryPrice(decimal totalPriceProducts)
    {
        decimal? deliveryCost;
        string deliveryMessage = string.Empty;

        var deliveryTiers = this.data.DeliveryPrices
                    .OrderBy(x => x.MinPriceOrder)
                    .ToList();

        var minDeliveryTier = deliveryTiers.First();
        var freeDeliveryTier = deliveryTiers.Last();

        var deliveryPriceTier = deliveryTiers
                    .FirstOrDefault(x =>
                     totalPriceProducts >= x.MinPriceOrder && totalPriceProducts <= x.MaxPriceOrder);

        if (deliveryPriceTier == null)
        {
            deliveryCost = null;
            deliveryMessage = $"Minimum order value for delivery is {minDeliveryTier.MinPriceOrder} lv";
        }
        else
        {
            deliveryCost = deliveryPriceTier.Price;
            if (deliveryPriceTier.Price > 0m)
            {
                deliveryMessage = $"Add items worth {freeDeliveryTier.MinPriceOrder - totalPriceProducts: 0.00}lv more to get a free delivery";
            }
            else
            {
                deliveryMessage = "You get free delivery!";
            }
        }
        return (deliveryCost, deliveryMessage);
    }
}
