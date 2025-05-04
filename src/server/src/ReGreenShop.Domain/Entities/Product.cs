using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Product : BaseDeletableModel<int>
{
    public Product()
    {
        ProductCategories = new HashSet<ProductCategory>();
        LabelProducts = new HashSet<LabelProduct>();
        CartItems = new HashSet<CartItem>();
        UserLikes = new HashSet<UserLikeProduct>();
        OrderDetails = new HashSet<OrderDetail>();
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

    public string? Packaging { get; set; }

    public string? OriginalUrl { get; set; }

    public Image? Image { get; set; }

    public IEnumerable<ProductCategory> ProductCategories { get; set; }

    public IEnumerable<LabelProduct> LabelProducts { get; set; }

    public IEnumerable<CartItem> CartItems { get; set; }

    public IEnumerable<UserLikeProduct> UserLikes { get; set; }

    public IEnumerable<OrderDetail> OrderDetails { get; set; }
}
