using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Identity;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Infrastructure.Persistence.Identity;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Services;
public class IdentityService : IIdentity
{
    private readonly ITokenGenerator tokenGenerator;
    private readonly UserManager<User> userManager;

    public IdentityService(ITokenGenerator tokenGenerator,
                           UserManager<User> userManager)
    {
        this.tokenGenerator = tokenGenerator;
        this.userManager = userManager;
    }

    public async Task<string?> GetUserName(string userId)
            => await this.userManager
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();
    public async Task<AuthResponse> LoginUserAsync(string username, string password)
    {
        var user = await this.userManager.FindByNameAsync(username)
            ?? throw new AuthenticationException("Invalid username or password.");

        var passwordValid = await this.userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
        {
            throw new AuthenticationException("Invalid username or password.");
        }
        var isAdmin = await this.userManager.IsInRoleAsync(user, AdminName);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? ""),
        };

        if (isAdmin)
        {
            claims.Add(new(ClaimTypes.Role, AdminName));
        }

        var accessToken = this.tokenGenerator.GenerateAccessToken(claims);
        return new AuthResponse
        {
            IsAdmin = isAdmin,
            UserId = user.Id,
            UserName = user.UserName ?? "",
            AccessToken = accessToken
        };
    }

    public async Task<AuthResponse> RegisterUserAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new AuthenticationException("Username is required.");
        }

        if (await this.userManager.FindByNameAsync(username) is not null)
        {
            throw new AuthenticationException("Username already exists.");
        }

        var user = new User { UserName = username };
        var result = await this.userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new AuthenticationException("Failed to create user account.");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? ""),
        };

        var accessToken = this.tokenGenerator.GenerateAccessToken(claims);

        return new AuthResponse
        {
            IsAdmin = false,
            UserId = user.Id,
            UserName = user.UserName ?? "",
            AccessToken = accessToken
        };
    }
}
