namespace ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;
public class UserInfoForOrderModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullAddress { get; set; }
    public int? TotalGreenPoints { get; set; }
}
