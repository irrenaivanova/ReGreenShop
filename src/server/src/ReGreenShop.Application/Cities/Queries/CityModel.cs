using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Cities.Queries;
public class CityModel : IMapFrom<City>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
