using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Infrastructure.Persistence;
using ReGreenShop.Infrastructure.Persistence.Identity;
using ReGreenShop.Infrastructure.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
          this IServiceCollection services,
          IConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)))
            .AddScoped<IData>(provider => provider.GetService<ApplicationDbContext>()!);

        services
            .AddDefaultIdentity<User>()
            .AddEntityFrameworkStores<ApplicationDbContext>();


        // Add JWtAuthentication

        services
           .AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
