using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class DiscountVouchersSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        if (data.DiscountVouchers.Any())
        {
            return;
        }

        var discounts = new List<(int points, decimal discount)>
        {
            (250, 5),
            (400, 10),
            (700, 20),
        };
        foreach (var item in discounts)
        {
            var discount = new DiscountVoucher()
            {
                GreenPoints = item.points,
                PriceDiscount = item.discount
            };
            await data.DiscountVouchers.AddAsync(discount);
        }
    }
}
