using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Infrastructure.Persistence;
using ReGreenShop.Infrastructure.Persistence.Identity;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using ReGreenShop.Infrastructure.Services;
using ReGreenShop.Infrastructure.Settings;
using Stripe;
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
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddConventionalServices(typeof(ServiceRegistration).Assembly);

        services
            .AddScoped<ApplicationDbContextSeeder>();

        // SendGrid
        services
            .Configure<SendGridSettings>(configuration.GetSection(nameof(SendGridSettings)));
        var sendSettings = configuration.GetSection(nameof(SendGridSettings)).Get<SendGridSettings>() ??
                            throw new InvalidOperationException("The SendGridSettings are missing!");
        var sendGridApiKey = sendSettings.ApiKey;

        services.AddTransient<IEmailSender, SendGridEmailSender>();

        // JWToken
        services
            .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        var jwtSettingsConfigSection = configuration.GetSection(nameof(JwtSettings));

        var jwtSettings = jwtSettingsConfigSection.Get<JwtSettings>() ??
            throw new InvalidOperationException("The JwtSettings are missing!");

        var signKey = Encoding.ASCII.GetBytes(jwtSettings.SignKey);

        // Stripe
        services
            .Configure<StripeSettings>(configuration.GetSection(nameof(StripeSettings)));

        var stripeSettingsConfigSection = configuration.GetSection(nameof(StripeSettings));

        var stripeSettings = stripeSettingsConfigSection.Get<StripeSettings>() ??
            throw new InvalidOperationException("The StripeSettings are missing!");

        StripeConfiguration.ApiKey = stripeSettings.SecretKey;


        // GoogleSettings
        services.Configure<GoogleAuthentication>(configuration.GetSection(nameof(GoogleAuthentication)));
        var googleSettings = configuration.GetSection(nameof(GoogleAuthentication)).Get<GoogleAuthentication>() ??
                     throw new InvalidOperationException("The GoogleAuthenticationSettings are missing!");
        var googleClientId = googleSettings.ClientId;
        var googleClientSecret = googleSettings.ClientSecret;

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddSignalR();
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(options =>
            {

                options.ClientId = googleClientId;
                options.ClientSecret = googleClientSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                options.Scope.Add("email");
                options.Scope.Add("profile");

                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    var redirectUri = context.RedirectUri;
                    redirectUri += (redirectUri.Contains("?") ? "&" : "?") + "prompt=select_account";

                    context.Response.Redirect(redirectUri);
                    return Task.CompletedTask;
                };

            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = JsonConvert.SerializeObject(new { error = "Unauthorized: Please provide valid credentials." });
                        return context.Response.WriteAsync(result);
                    },

                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
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
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
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
