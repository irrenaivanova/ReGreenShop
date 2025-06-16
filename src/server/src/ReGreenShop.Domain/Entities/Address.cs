using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Address : BaseDeletableModel<int>
{
    public Address()
    {
        Orders = new List<Order>();
    }

    public string? FullAddress { get; set; }

    public int? CityId { get; set; }

    public City? City { get; set; } = default!;

    public string? Street { get; set; } 

    public int? Number { get; set; }

    public string UserId { get; set; } = string.Empty;

    public IList<Order> Orders { get; set; }
}
