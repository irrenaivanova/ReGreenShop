using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Image : BaseDeletableModel<int>
{
    public string Name { get; set; } = string.Empty;

    public string? LocalPath { get; set; }

    public string? BlobPath { get; set; }

    public int? ProductId { get; set; }

    public Product? Product { get; set; }

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
}
