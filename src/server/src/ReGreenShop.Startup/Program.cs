var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// !!! Change WeatherForecast with real class
builder.Services.AddControllers().
    AddApplicationPart(typeof(ReGreenShop.Web.WeatherForecast).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
