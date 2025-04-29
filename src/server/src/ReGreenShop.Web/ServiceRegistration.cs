using Microsoft.Extensions.DependencyInjection;
using static ReGreenShop.Application.ServiceRegistration;

namespace ReGreenShop.Web;
public static class ServiceRegistration
{
    public static IServiceCollection AddWebComponents(
    this IServiceCollection services)
    => services
        .AddHttpContextAccessor()
        .AddConventionalServices(typeof(ServiceRegistration).Assembly);
}
