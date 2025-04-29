using ReGreenShop.Infrastructure.Persistence.Seeding.Common;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
public class ProductsSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ReGreenShop.Infrastructure/Persistence/Seeding/Common/SeedFiles/products.json");
        //var json = await File.ReadAllTextAsync(filePath);
        //var products = JsonConvert.DeserializeObject<List<Product>>(json);

        throw new NotImplementedException();
    }
}
