namespace ReGreenShop.Infrastructure.Persistence.Seeding.Common;
public class ProductDto
{
    public ProductDto()
    {
        this.Categories = new HashSet<string>();
    }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? Weight { get; set; }

    public decimal Price { get; set; }

    public string? ProductCode { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public string ImageUrl { get; set; }

    public IEnumerable<string> Categories { get; set; }
}
