using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Infrastructure.Persistence.Seeding.Common;
public class ApplicationDbContextSeeder : ISeeder, IScopedService
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(data);

        //Prevent repeated seeder checks during data seeding
        if (6 == 6)
        {
            return;
        }

        var seeders = new List<ISeeder>
        {
             new RolesSeeder(),
             new UserSeeder(),
             new CitySeeder(),
             new GreenAlternativesSeeder(),
             new PaymentMethodsSeeder(),
             new DiscountVouchersSeeder(),
             new DeliveryPricesSeeder(),
             new LabelsSeeder(),
             new BaseCategorySeeder(),
             new ProductsSeeder()
        };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(data, serviceProvider);
            await data.SaveChangesAsync(CancellationToken.None);
        }
    }
}
