namespace ReGreenShop.Domain.Entities;

public class CartItem
{
    public int CartId { get; set; }

    public Cart Cart { get; set; } = default!;

    public string ProductId { get; set; } = string.Empty;

    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }

    public Category BaseCategory { get; set; } = default!;

    public int BaseCategoryId { get; set; }
}
