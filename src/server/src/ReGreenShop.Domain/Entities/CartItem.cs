namespace ReGreenShop.Domain.Entities;

public class CartItem
{
    public string CartId { get; set; } = string.Empty;

    public Cart Cart { get; set; } = default!;

    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }

    public Category BaseCategory { get; set; } = default!;

    public int BaseCategoryId { get; set; }
}
