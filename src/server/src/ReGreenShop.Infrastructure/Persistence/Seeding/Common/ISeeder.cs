namespace ReGreenShop.Infrastructure.Persistence.Seeding.Common;
public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
}
