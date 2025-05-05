using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Infrastructure.Persistence.Seeding.Common;
public class ApplicationDbContextSeeder : ISeeder, IScopedService
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(data);

        var seeders = new List<ISeeder>
        {
            // new ProductsSeeder(),
             new RolesSeeder(),
             new UserSeeder(),
             new CitySeeder(),
             new BaseCategorySeeder(),
        };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(data, serviceProvider);
            await data.SaveChangesAsync(CancellationToken.None);
        }
    }
}
