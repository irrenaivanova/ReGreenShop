using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Category : BaseDeletableModel<int>
{
    public Category()
    {  
        this.ProductCategories = new HashSet<ProductCategory>();
        this.CartItems = new HashSet<CartItem>();
        this.OrderDetails = new HashSet<OrderDetail>();
    }

    public string Name { get; set; } = string.Empty;

    public int? ParentCategoryId { get; set; }

    public Category? ParentCategory { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public IEnumerable<ProductCategory> ProductCategories { get; set; }

    public IEnumerable<CartItem> CartItems { get; set; }

    public IEnumerable<OrderDetail> OrderDetails { get; set; }
}
