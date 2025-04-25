using ReGreenShop.Domain.common;
using ReGreenShop.Domain.Entities.Enum;

namespace ReGreenShop.Domain.Entities;
public class Order : BaseDeletableModel<string>
{
    public Order()
    {
        Id = Guid.NewGuid().ToString();
        OrderDetails = new HashSet<OrderDetail>();
        OrderGreenAlternativeDetails = new HashSet<OrderGreenAlternativeDetail>();
    }

    public string InvoiceUrl { get; set; } = string.Empty;

    public string UserId { get; set; } = default!;

    public string TotalPrice { get; set; } = string.Empty;

    public OrderStatus Status { get; set; }

    public int DeliveryPriceId { get; set; }

    public DeliveryPrice DeliveryPrice { get; set; } = default!;

    public int AddressId { get; set; }

    public Address Address { get; set; } = default!;

    public int PaymentId { get; set; }

    public PaymentDetail Payment { get; set; } = default!;

    public int? UsedPointsToDiscountId { get; set; }

    public PointsToDiscount? UsedPointsToDiscount { get; set; }

    public IEnumerable<OrderDetail> OrderDetails { get; set; }

    public IEnumerable<OrderGreenAlternativeDetail> OrderGreenAlternativeDetails { get; set; }
}
