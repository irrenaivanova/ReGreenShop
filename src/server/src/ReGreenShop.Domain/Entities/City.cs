using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class City : BaseDeletableModel<int>
{
    public City()
    {
        Addresses = new List<Address>();
    }

    public string Name { get; set; } = string.Empty;

    public IList<Address> Addresses { get; set; }
}
