using ReGreenShop.Domain.common;
using ReGreenShop.Domain.Entities.Enum;

namespace ReGreenShop.Domain.Entities;

public class Payment : BaseDeletableModel<string>
{
    public Payment()
    {
        Id = Guid.NewGuid().ToString();
    }

    public PaymentStatus Status { get; set; }

    public int PaymentMethodId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = default!;

    public string OrderId { get; set; } = string.Empty;

    public Order Order { get; set; } = default!;
}
