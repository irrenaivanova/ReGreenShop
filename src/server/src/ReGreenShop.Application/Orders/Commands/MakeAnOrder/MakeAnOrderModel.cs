namespace ReGreenShop.Application.Orders.Commands.MakeAnOrder;
public class MakeAnOrderModel
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int CityId { get; set; }

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public int PaymentMethodId { get; set; }

    public DateTime DeliveryDateTime { get; set; }

    public int? DiscountVoucherId { get; set; }

}
