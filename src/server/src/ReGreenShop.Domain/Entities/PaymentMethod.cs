using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class PaymentMethod : BaseDeletableModel<int>
{
    public PaymentMethod()
    {
        Payments = new HashSet<Payment>();
    }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<Payment> Payments { get; set; }
}
