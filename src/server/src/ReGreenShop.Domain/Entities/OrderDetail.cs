using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class OrderDetail : IDeletableEntity
{
    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;

    public string OrderId { get; set; } = string.Empty;

    public Order Order { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public decimal TotalPrice { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Category? BaseCategory { get; set; } = default!;

    public int? BaseCategoryId { get; set; }
}
