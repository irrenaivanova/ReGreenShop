using System.Security.Claims;
using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface ITokenGenerator : IService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
}
