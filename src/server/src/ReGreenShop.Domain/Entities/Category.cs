using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Category : BaseDeletableModel<int>
{
    public Category()
    {
        Products = new HashSet<Product>();
    }

    public string Name { get; set; } = string.Empty;

    public int? ParentCategoryId { get; set; }

    public Category? ParentCategory { get; set; }

    public IEnumerable<Product> Products { get; set; }
}
