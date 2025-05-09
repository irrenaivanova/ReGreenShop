using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Product : BaseDeletableModel<int>
{
    public Product()
    {
        ProductCategories = new List<ProductCategory>();
        LabelProducts = new List<LabelProduct>();
        CartItems = new List<CartItem>();
        UserLikes = new List<UserLikeProduct>();
        OrderDetails = new List<OrderDetail>();
    }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? ProductCode { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public string? Packaging { get; set; }

    public string? OriginalUrl { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public IList<ProductCategory> ProductCategories { get; set; }

    public IList<LabelProduct> LabelProducts { get; set; }

    public IList<CartItem> CartItems { get; set; }

    public IList<UserLikeProduct> UserLikes { get; set; }

    public IList<OrderDetail> OrderDetails { get; set; }
}
