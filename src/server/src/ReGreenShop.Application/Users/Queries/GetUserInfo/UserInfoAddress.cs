using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Users.Queries.GetUserInfo;
public class UserInfoAddress : IMapFrom<Address>
{
    public string? Street { get; set; }

    public int? Number { get; set; }

    public string? CityName { get; set; }
}
