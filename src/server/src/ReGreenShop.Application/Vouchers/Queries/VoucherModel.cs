using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Vouchers.Queries;
public class VoucherModel : IMapFrom<DiscountVoucher>
{
    public int Id { get; set; }

    public int GreenPoints { get; set; }

    public decimal PriceDiscount { get; set; }
}
