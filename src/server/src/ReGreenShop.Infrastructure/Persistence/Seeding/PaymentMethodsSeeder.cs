using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class PaymentMethodsSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        if(data.PaymentMethods.Any())
        {
            return;
        }
        List<string> methods = new List<string>()
                       { "Online Payment with Card",
                       "Cash on Delivery",
                       "Card Payment on Delivery",
                       "Pay with Coupon on Delivery" };

        foreach (var method in methods)
        {
            var newMethod = new PaymentMethod()
            {
                Name = method
            };
            await data.PaymentMethods.AddAsync(newMethod);
        }
    }
}
