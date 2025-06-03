using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Infrastructure.Persistence.Identity;

namespace ReGreenShop.Infrastructure.Services;
public class ExternalAuthService : IExternalAuth
{
    private readonly SignInManager<User> signInManager;

    public ExternalAuthService(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    public Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
    {
        return this.signInManager.GetExternalLoginInfoAsync()!;
    }

    public AuthenticationProperties GetGoogleAuthProperties(string redirectUrl)
    {
        return this.signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
    }
}
