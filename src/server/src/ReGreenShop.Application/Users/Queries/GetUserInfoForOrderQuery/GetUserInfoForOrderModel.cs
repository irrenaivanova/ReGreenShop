using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;
public class GetUserInfoForOrderModel 
{
    public string? FirstName { get; set; } 

    public string? LastName { get; set; }

    public string? Street { get; set; }

    public int? CityId { get; set; }

    public int? TotalGreenPoints { get; set; }
}
