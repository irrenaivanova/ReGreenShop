using Microsoft.Extensions.DependencyInjection;
using static ReGreenShop.Application.ServiceRegistration;

namespace ReGreenShop.Web;
public static class ServiceRegistration
{
    public static IServiceCollection AddWebComponents(
    this IServiceCollection services)
    {
        services
                .AddHttpContextAccessor()
                .AddConventionalServices(typeof(ServiceRegistration).Assembly);

        services.AddSession(options =>
        {
            options.Cookie.Name = "ReGreenShop.Session";
            options.IdleTimeout = TimeSpan.FromDays(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        return services;
    }
}
