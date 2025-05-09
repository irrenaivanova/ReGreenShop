using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class DiscountVoucher : BaseDeletableModel<int>
{
    public DiscountVoucher()
    {
        Orders = new List<Order>();
    }

    public int GreenPoints { get; set; }

    public decimal PriceDiscount { get; set; }

    public IList<Order> Orders { get; set; } = default!;
}
