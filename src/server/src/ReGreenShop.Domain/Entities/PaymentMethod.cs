using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;
public class PaymentMethod : BaseDeletableModel<int>
{
    public PaymentMethod()
    {
        PaymentDetails = new HashSet<PaymentDetail>();
    }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<PaymentDetail> PaymentDetails { get; set; }
}
