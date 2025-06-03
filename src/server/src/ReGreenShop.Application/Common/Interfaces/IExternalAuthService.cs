using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IExternalAuth : IService
{
    Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
    AuthenticationProperties GetGoogleAuthProperties(string redirectUrl);
}
