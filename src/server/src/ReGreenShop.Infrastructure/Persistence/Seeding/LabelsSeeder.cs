using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class LabelsSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        if (data.Labels.Any())
        {
            return;
        }
        List<string> labels = new List<string>()
                       { "Top Offer",
                       "Limited",
                       "New Arrival",
                       "Last Chance",
                       "Two for One"};

        foreach (var label in labels)
        {
            var newLabel = new Label()
            {
                Name = label
            };
            await data.Labels.AddAsync(newLabel);
        }
    }
}
