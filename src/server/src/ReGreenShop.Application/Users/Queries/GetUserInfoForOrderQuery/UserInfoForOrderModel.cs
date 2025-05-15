namespace ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;
public class UserInfoForOrderModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Street { get; set; }

    public int? Number { get; set; }

    public int? CityId { get; set; }

    public string? CityName { get; set; }

    public int? TotalGreenPoints { get; set; }
}
