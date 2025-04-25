using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;
public class City : BaseDeletableModel<int>
{
    public City()
    {
        Addresses = new HashSet<Address>();
    }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<Address> Addresses { get; set; }
}
