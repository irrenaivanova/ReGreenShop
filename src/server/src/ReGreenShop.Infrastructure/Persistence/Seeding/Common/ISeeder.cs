using Microsoft.AspNetCore.Hosting;

namespace ReGreenShop.Infrastructure.Persistence.Seeding.Common;
public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider);
}
