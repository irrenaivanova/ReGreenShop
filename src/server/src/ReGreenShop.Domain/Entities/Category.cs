using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Category : BaseDeletableModel<int>
{
    public Category()
    {
        ProductCategories = new List<ProductCategory>();
        CartItems = new List<CartItem>();
        OrderDetails = new List<OrderDetail>();
        ChildCategories = new List<Category>();
    }

    public string? NameInBulgarian { get; set; }

    public string? NameInEnglish { get; set; }

    public int? ParentCategoryId { get; set; }

    public Category? ParentCategory { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public IList<ProductCategory> ProductCategories { get; set; }

    public IList<CartItem> CartItems { get; set; }

    public IList<OrderDetail> OrderDetails { get; set; }

    public IList<Category> ChildCategories { get; set; }
}
