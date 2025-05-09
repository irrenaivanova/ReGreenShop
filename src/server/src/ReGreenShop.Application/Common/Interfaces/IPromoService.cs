using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IPromo : IService
{
    Task RefreshWeeklyPromosAsync();
}
