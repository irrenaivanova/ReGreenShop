using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Infrastructure.Persistence.Seeding.Common;
public class ApplicationDbContextSeeder : ISeeder, IScopedService
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        var seeders = new List<ISeeder>
        {
            // new ProductsSeeder(),
             new RolesSeeder(),
             new CitySeeder(),
             new BaseCategorySeeder(),
        };

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(dbContext, serviceProvider);
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
