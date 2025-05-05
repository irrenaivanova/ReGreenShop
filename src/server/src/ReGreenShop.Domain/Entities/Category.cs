using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Category : BaseDeletableModel<int>
{
    public Category()
    {
        ProductCategories = new HashSet<ProductCategory>();
        CartItems = new HashSet<CartItem>();
        OrderDetails = new HashSet<OrderDetail>();
    }

    public string? NameInBulgarian { get; set; }

    public string? NameInEnglish { get; set; }

    public int? ParentCategoryId { get; set; }

    public Category? ParentCategory { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public IEnumerable<ProductCategory> ProductCategories { get; set; }

    public IEnumerable<CartItem> CartItems { get; set; }

    public IEnumerable<OrderDetail> OrderDetails { get; set; }
}
