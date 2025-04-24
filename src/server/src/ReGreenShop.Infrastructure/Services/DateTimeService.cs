using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Infrastructure.Services;
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
