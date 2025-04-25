using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;
public class Address : BaseDeletableModel<int>
{
    public Address()
    {
        Orders = new HashSet<Order>();
    }

    public int CityId { get; set; }

    public City City { get; set; } = default!;

    public string Street { get; set; } = string.Empty;

    public int Number { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; }
}
