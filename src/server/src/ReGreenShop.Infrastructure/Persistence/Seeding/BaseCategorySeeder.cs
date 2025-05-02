using ReGreenShop.Application.Common.Utilities;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
public class BaseCategorySeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (dbContext.Categories.Any())
        {
            return;
        }
        var categories = new List<string>
        {
            "Bread and Pastries",
            "Fruits and Vegetables",
            "Meat and Fish",
            "Sausages and Delicacies",
            "Dairy and Eggs",
            "Frozen Foods",
            "Packaged Foods",
            "Beverages",
            "Organic and Specialty",
            "Cosmetics",
            "For the Home",
            "For the Baby",
            "Pets"
        };

        for (int i = 0; i < categories.Count; i++)
        {
            var newCategory = new Category()
            {
                Name = categories[i],
                Image = new Image()
                {
                    Name = GenerateSlugName(categories[i]),
                    LocalPath = GeneratePath(categories[i]),
                }
            };

            await dbContext.Categories.AddAsync(newCategory);
        }
    }

    private string GenerateSlugName(string name)
    {
        var slugGenerator = new SlugGenerator();
        return slugGenerator.GenerateSlug(name);
    }
    private string GeneratePath(string name)
    {
        return $"{BaseUrl}/images/categories/{GenerateSlugName(name)}.png";
    }
}
