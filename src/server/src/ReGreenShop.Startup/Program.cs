using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Categories.Queries.GetRootCategories;
using ReGreenShop.Infrastructure.Persistence;
using ReGreenShop.Infrastructure.Persistence.Seeding.Common;
using ReGreenShop.Web;
using static ReGreenShop.Web.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

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
// builder.Services.AddApplication(builder.Configuration);
builder.Services.AddWebComponents();

builder.Services.AddControllers().
    AddApplicationPart(typeof(ReGreenShop.Web.Controllers.BaseController).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// when fix addApplication this should be cleared
builder.Services.AddMediatR(typeof(GetRootCategoriesQuery).Assembly);

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
