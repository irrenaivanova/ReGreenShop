using Hangfire;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using ReGreenShop.Application.Categories.Queries.GetRootCategories;
using ReGreenShop.Application.Common;
using ReGreenShop.Application.Common.Behaviors;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Infrastructure.Jobs;
using ReGreenShop.Infrastructure.Persistence;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using ReGreenShop.Startup.AuthorizationFilter;
using ReGreenShop.Web;
using ReGreenShop.Web.Middleware;
using Serilog;
using static ReGreenShop.Application.ServiceRegistration;
using static ReGreenShop.Web.ServiceRegistration;
using static ReGreenShop.Application.Common.GlobalConstants;
using ReGreenShop.Application.Common.Services;
using ReGreenShop.Web.ModelBinders;

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
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddWebComponents();


builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
})
.AddApplicationPart(typeof(ReGreenShop.Web.Controllers.BaseController).Assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerJwToken();

builder.Services.AddMediatR(typeof(GetRootCategoriesQuery).Assembly);
builder.Services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));

builder.Host.UseSerilog();

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnectionHangFire")));
builder.Services.AddHangfireServer();
builder.Services.AddTransient<PromoJob>();


var app = builder.Build();

// QuestPDF is free if the organization earns less than $1million USD per year
QuestPDF.Settings.License = LicenseType.Community;


// Perform database migration and seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeeder>();
    await seeder.SeedAsync(dbContext, scope.ServiceProvider);
}
AutoMapperConfig.RegisterMappings(typeof(GlobalConstants).Assembly);

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

if (app.Environment.IsProduction())
{
    app.UseHangfireDashboard(
        "/hangfire",
        new DashboardOptions { Authorization = new[] { new HangFireAuthorizationFilter() } });
}

// Adding the requiring job after building the app - it don`t modify the service collection
RecurringJob.AddOrUpdate<PromoJob>("Promotion Service", service => service.Execute(CancellationToken.None), Cron.Weekly);
RecurringJob.AddOrUpdate<DeleteCartsJob>("DeleteUnUsedCarts Service", service => service.Execute(CancellationToken.None), Cron.Daily);

app.UseCustomExceptionHandler();
    
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


