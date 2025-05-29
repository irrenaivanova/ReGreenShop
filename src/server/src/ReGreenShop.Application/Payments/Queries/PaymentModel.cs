using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Payments.Queries;
public class PaymentModel : IMapFrom<PaymentMethod>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
