using System.ComponentModel;

namespace ReGreenShop.Application.Orders.Commands.MakeAnOrder;
public class MakeAnOrderModel
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullAddress { get; set; } = string.Empty;

    public int PaymentMethodId { get; set; }

    public DateTime DeliveryDateTime { get; set; }

    [DefaultValue(null)]
    public int? DiscountVoucherId { get; set; }

}
