using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Product : BaseDeletableModel<int>
{
    public Product()
    {
        this.Categories = new HashSet<Category>();
        this.LabelProducts = new HashSet<LabelProduct>();
        this.CartItems = new HashSet<CartItem>();
        this.UserLikes = new HashSet<string>();
    }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? Weight { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? ProductCode { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public IEnumerable<Category> Categories { get; set; }

    public IEnumerable<LabelProduct> LabelProducts { get; set; }

    public IEnumerable<CartItem> CartItems { get; set; }

    public IEnumerable<string> UserLikes { get; set; }
}
