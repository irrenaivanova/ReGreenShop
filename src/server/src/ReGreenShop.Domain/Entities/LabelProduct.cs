namespace ReGreenShop.Domain.Entities;

public class LabelProduct
{
    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;

    public int LabelId { get; set; }

    public Label Label { get; set; } = default!;

    // in days
    public int? Duration { get; set; }

    public int? PercentageDiscount { get; set; }
}
