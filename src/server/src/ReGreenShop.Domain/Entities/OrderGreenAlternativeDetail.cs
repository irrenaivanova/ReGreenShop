namespace ReGreenShop.Domain.Entities;

public class OrderGreenAlternativeDetail
{
    public string OrderId { get; set; } = string.Empty;

    public Order Order { get; set; } = default!;

    public int GreenAlternativeId { get; set; }

    public GreenAlternative GreenAlternative { get; set; } = default!;

    public int Quantity { get; set; }
}
