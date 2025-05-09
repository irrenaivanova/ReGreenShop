using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class LabelProduct : IAuditableEntity
{
    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;

    public int LabelId { get; set; }

    public Label Label { get; set; } = default!;

    // in days
    public double Duration { get; set; }

    public int? PercentageDiscount { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
