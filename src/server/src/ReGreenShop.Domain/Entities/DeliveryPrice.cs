using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class DeliveryPrice : BaseDeletableModel<int>
{
    public DeliveryPrice()
    {
        this.Orders = new HashSet<Order>();
    }

    public decimal MinPriceOrder { get; set; }

    public decimal MaxPriceOrder { get; set; }

    public decimal Price { get; set; }

    public IEnumerable<Order> Orders { get; set; }
}
