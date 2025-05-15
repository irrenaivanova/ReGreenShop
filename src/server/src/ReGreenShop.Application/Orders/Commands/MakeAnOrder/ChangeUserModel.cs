namespace ReGreenShop.Application.Orders.Commands.MakeAnOrder;
public class ChangeUserModel
{
    public  int  GreenPoints {get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public int AddressId { get; set; }

}
