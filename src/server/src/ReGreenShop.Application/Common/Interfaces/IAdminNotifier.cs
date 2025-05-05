using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IAdminNotifier : IService
{
    Task NotifyAsync(string title, string message);
}
