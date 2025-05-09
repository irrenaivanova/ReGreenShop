using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class PaymentMethod : BaseDeletableModel<int>
{
    public PaymentMethod()
    {
        Payments = new List<Payment>();
    }

    public string Name { get; set; } = string.Empty;

    public IList<Payment> Payments { get; set; }
}
