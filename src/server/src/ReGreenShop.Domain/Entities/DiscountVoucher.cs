using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class DiscountVoucher : BaseDeletableModel<int>
{
    public DiscountVoucher()
    {
        this.Orders = new List<Order>();
    }

    public int GreenPoints { get; set; }

    public decimal PriceDiscount { get; set; }

    public IEnumerable<Order> Orders { get; set; } = default!;
}
