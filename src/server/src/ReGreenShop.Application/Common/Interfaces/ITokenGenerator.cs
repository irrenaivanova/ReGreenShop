using System.Security.Claims;

namespace ReGreenShop.Application.Common.Interfaces;
public interface ITokenGenerator
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
}
