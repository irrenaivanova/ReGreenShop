using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Infrastructure.Settings;

namespace ReGreenShop.Infrastructure.Services;
public class TokenGeneratorService : ITokenGenerator
{
    private readonly JwtSettings jwtSettings;

    public TokenGeneratorService(IOptions<JwtSettings> jwtOptions)
    {
        this.jwtSettings = jwtOptions.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.SignKey));
        var signingCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(120),
            SigningCredentials = signingCredentials,
            Issuer = this.jwtSettings.Issuer,
            Audience = this.jwtSettings.Audience,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
