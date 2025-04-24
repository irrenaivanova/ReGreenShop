using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;
public class Product : BaseDeletableModel<int>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? Weight { get; set; }

    public decimal Price { get; set; }
}
