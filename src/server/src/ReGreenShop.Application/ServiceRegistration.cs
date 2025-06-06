using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ReGreenShop.Application.Common.Behaviors;
using ReGreenShop.Application.Common.Identity.Login;
using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    => services
        .AddAutoMapper(Assembly.GetExecutingAssembly())
        .AddValidatorsFromAssemblyContaining<LoginCommandValidator>()
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

    public static IServiceCollection AddConventionalServices(
           this IServiceCollection services,
           Assembly assembly)
    {
        var serviceInterfaceType = typeof(IService);
        var singletonServiceInterfaceType = typeof(ISingletonService);
        var scopedServiceInterfaceType = typeof(IScopedService);

        var types = assembly
            .GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Service = t.GetInterface($"I{t.Name.Replace("Service", string.Empty)}"),
                Implementation = t
            })
            .Where(t => t.Service != null);

        foreach (var type in types)
        {
            if (serviceInterfaceType.IsAssignableFrom(type.Service))
            {
                services.AddTransient(type.Service, type.Implementation);
            }
            else if (singletonServiceInterfaceType.IsAssignableFrom(type.Service))
            {
                services.AddSingleton(type.Service, type.Implementation);
            }
            else if (scopedServiceInterfaceType.IsAssignableFrom(type.Service))
            {
                services.AddScoped(type.Service, type.Implementation);
            }
        }

        return services;
    }
}
