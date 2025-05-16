using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface INotifier : IService
{
    Task NotifyAdminAsync(string title, string message);

    Task NotifyUserAsync(string userId, string title, string message);
}
