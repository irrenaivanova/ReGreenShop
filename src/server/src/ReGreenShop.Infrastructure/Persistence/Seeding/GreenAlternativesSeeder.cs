using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class GreenAlternativesSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        if (data.GreenAlternatives.Any())
        {
            return;
        }

        var greenAlternatives = new List<(string Name, int Points)>
        {
            ("1 plastic bottle", 5),
            ("1 glass bottle", 5),
            ("1 metal can", 5),
            ("1 delivery paper bag", 1),
            ("1 plastic freezer bag (~22×33cm) full of plastic caps", 20)
        };

        foreach (var alternative in greenAlternatives)
        {
            var greenAlt = new GreenAlternative()
            {
                Name = alternative.Name,
                RewardPoints = alternative.Points,
            };

            await data.GreenAlternatives.AddAsync(greenAlt);
        }
    }
}
