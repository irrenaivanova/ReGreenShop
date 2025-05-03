namespace ReGreenShop.Application.Common.Identity;
public interface IIdentity
{
    Task<AuthResponse> LoginUserAsync(string username, string password);

    Task<AuthResponse> RegisterUserAsync(string username, string password);
}
