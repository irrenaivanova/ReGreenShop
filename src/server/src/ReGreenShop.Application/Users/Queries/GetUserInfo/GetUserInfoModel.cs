namespace ReGreenShop.Application.Users.Queries.GetUserInfo;
public class GetUserInfoModel
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int TotalGreenPoints { get; set; }

    public IEnumerable<UserInfoAddress> Addresses { get; set; } = new List<UserInfoAddress>();

}
