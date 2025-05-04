using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Categories.Queries.GetRootCategories;
using ReGreenShop.Application.Common.Behaviors;
using ReGreenShop.Infrastructure.Persistence;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using ReGreenShop.Web;
using ReGreenShop.Web.Middleware;
using Serilog;
using static ReGreenShop.Application.ServiceRegistration;
using static ReGreenShop.Web.ServiceRegistration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddWebComponents();

builder.Services.AddControllers().
    AddApplicationPart(typeof(ReGreenShop.Web.Controllers.BaseController).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerJwToken();

builder.Services.AddMediatR(typeof(GetRootCategoriesQuery).Assembly);
builder.Services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));

builder.Host.UseSerilog();

var app = builder.Build();

// Perform database migration and seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeeder>();
    await seeder.SeedAsync(dbContext, scope.ServiceProvider);
}

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("swagger/v1/swagger.json", "ReGreenShop API v1");
        c.RoutePrefix = string.Empty;  // Makes Swagger UI available at the root URL
    });
}
app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
