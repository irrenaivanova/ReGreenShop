using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class DeliveryPrice : BaseDeletableModel<int>
{
    public DeliveryPrice()
    {
        Orders = new List<Order>();
    }

    public decimal MinPriceOrder { get; set; }

    public decimal MaxPriceOrder { get; set; }

    public decimal Price { get; set; }

    public IList<Order> Orders { get; set; }
}
