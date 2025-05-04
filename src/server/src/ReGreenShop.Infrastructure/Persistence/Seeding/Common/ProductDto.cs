namespace ReGreenShop.Infrastructure.Persistence.Seeding.Common;
public class ProductDto
{
    public ProductDto()
    {
        this.Categories = new List<string>();
    }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public string? Packaging { get; set; }

    public string? OriginalUrl { get; set; }

    public string? ImageUrl { get; set; }

    public IEnumerable<string> Categories { get; set; }
}
