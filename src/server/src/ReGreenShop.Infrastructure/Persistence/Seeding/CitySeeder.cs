using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class CitySeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (dbContext.Cities.Any())
        {
            return;
        }

        var cities = new List<string>() { "Sofia", "Plovdiv", "Varna" };

        foreach (var city in cities)
        {
            var newCity = new City()
            {
                Name = city,
            };
            await dbContext.Cities.AddAsync(newCity);
        }
    }
}
