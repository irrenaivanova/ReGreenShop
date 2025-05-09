using ReGreenShop.Domain.common;
using ReGreenShop.Domain.Entities.Enum;

namespace ReGreenShop.Domain.Entities;

public class Order : BaseDeletableModel<string>
{
    public Order()
    {
        Id = Guid.NewGuid().ToString();
        OrderDetails = new List<OrderDetail>();
        OrderGreenAlternativeDetails = new List<OrderGreenAlternativeDetail>();
    }

    public string InvoiceUrl { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string TotalPrice { get; set; } = string.Empty;

    public OrderStatus Status { get; set; }

    public int DeliveryPriceId { get; set; }

    public DeliveryPrice DeliveryPrice { get; set; } = default!;

    public int AddressId { get; set; }

    public Address Address { get; set; } = default!;

    public int PaymentId { get; set; }

    public Payment Payment { get; set; } = default!;

    public int? DiscountVoucherId { get; set; }

    public DiscountVoucher? DiscountVoucher { get; set; }

    public IList<OrderDetail> OrderDetails { get; set; }

    public IList<OrderGreenAlternativeDetail> OrderGreenAlternativeDetails { get; set; }
}
