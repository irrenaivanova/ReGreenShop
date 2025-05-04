using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Identity;
public interface IIdentity : IScopedService
{
    Task<AuthResponse> LoginUserAsync(string username, string password);

    Task<AuthResponse> RegisterUserAsync(string username, string password);

    Task<string?> GetUserName(string userId);
}
