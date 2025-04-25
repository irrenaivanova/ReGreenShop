using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;
public class PointsToDiscount : BaseDeletableModel<int>
{
    public int GreenPoints { get; set; }

    public decimal PriceDiscount { get; set; }

    public string OrderId { get; set; } = string.Empty;

    public Order Order { get; set; } = default!;
}
