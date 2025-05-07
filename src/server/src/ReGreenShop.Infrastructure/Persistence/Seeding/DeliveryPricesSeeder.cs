using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class DeliveryPricesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        if (data.DeliveryPrices.Any())
        {
            return;
        }
        var delivery = new List<(decimal minPrice, decimal maxPrice, decimal Price)>
        {
            (40m,69.99m,6m),
            (70m,99.99m,3m),
            (100m,10000m,0m)
        };

        foreach (var item in delivery)
        {
            var price = new DeliveryPrice()
            {
                MinPriceOrder = item.minPrice,
                MaxPriceOrder = item.maxPrice,
                Price = item.Price
            };
            await data.DeliveryPrices.AddAsync(price);
        }
    }
}
