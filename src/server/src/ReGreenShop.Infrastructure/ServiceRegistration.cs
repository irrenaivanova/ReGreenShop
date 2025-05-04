using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Infrastructure.Persistence;
using ReGreenShop.Infrastructure.Persistence.Identity;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using ReGreenShop.Infrastructure.Settings;
using static ReGreenShop.Application.ServiceRegistration;

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
            .AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddConventionalServices(typeof(ServiceRegistration).Assembly);

        services
            .AddScoped<ApplicationDbContextSeeder>();

        services
            .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        var jwtSettingsConfigSection = configuration.GetSection(nameof(JwtSettings));

        var jwtSettings = jwtSettingsConfigSection.Get<JwtSettings>() ??
            throw new InvalidOperationException("The JwtSettings are missing!");

        var signKey = Encoding.ASCII.GetBytes(jwtSettings.SignKey);

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    // Only for testing through Swagger
                    ValidateAudience = false,
                    // ValidateAudience = true,
                    ValidIssuer = jwtSettings.Issuer,
                    // ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signKey),
                    // The token will be considered expired immediately once it passes its expiration time
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    public static IServiceCollection AddSwaggerJwToken(
          this IServiceCollection services)
    {
        services.AddSwaggerGen(x =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            x.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            x.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}
