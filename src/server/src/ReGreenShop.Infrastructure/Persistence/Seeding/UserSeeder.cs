using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Seeding;
internal class UserSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext data, IServiceProvider serviceProvider)
    {
        if (data.Users.Any())
        {
            return;
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        string adminEmail = AdminEmail;
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = adminEmail,
                UserName = adminEmail,
                Cart = new Cart(),
            };
            var result = await userManager.CreateAsync(adminUser, adminUser.Email);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, AdminName);
            }
        }
    }
}
