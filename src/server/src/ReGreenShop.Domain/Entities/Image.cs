using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Image : BaseDeletableModel<int>
{
    public string Name { get; set; } = string.Empty;

    public string? LocalPath { get; set; }

    public string? BlobPath { get; set; }
}
