using ReGreenShop.Application.Common.Utilities;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class BaseCategorySeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (dbContext.Categories.Any())
        {
            return;
        }
        var categories = new List<(string English, string Bulgarian)>
        {
            ("Bread and Pastries", "Хляб и тестени"),
            ("Fruits and Vegetables", "Плодове и зеленчуци"),
            ("Meat and Fish", "Месо и риба"),
            ("Sausages and Delicacies", "Колбаси и деликатеси"),
            ("Dairy and Eggs", "Млечни и яйца"),
            ("Frozen Foods", "Замразени храни"),
            ("Packaged Foods", "Пакетирани храни"),
            ("Beverages", "Напитки"),
            ("Organic and Specialty", "Био и специализирани"),
            ("Cosmetics", "Козметика"),
            ("For the Home", "За дома и офиса"),
            ("For the Baby", "За бебето"),
            ("Pets", "Домашни любимци")
        };

        for (int i = 0; i < categories.Count; i++)
        {
            var newCategory = new Category()
            {
                NameInEnglish = categories[i].English,
                NameInBulgarian = categories[i].Bulgarian,
                Image = new Image()
                {
                    Name = GenerateSlugName(categories[i].English),
                    LocalPath = GeneratePath(categories[i].English),
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
        return $"/images/categories/{GenerateSlugName(name)}.png";
    }
}
