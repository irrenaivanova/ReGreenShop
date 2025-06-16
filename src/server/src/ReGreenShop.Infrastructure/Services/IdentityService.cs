using System.Linq.Expressions;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Identity;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Orders.Commands.MakeAnOrder;
using ReGreenShop.Application.Users.Queries.GetUserInfo;
using ReGreenShop.Application.Users.Queries.GetUserInfoForOrderQuery;
using ReGreenShop.Infrastructure.Persistence.Identity;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Services;
public class IdentityService : IIdentity
{
    private readonly ITokenGenerator tokenGenerator;
    private readonly UserManager<User> userManager;
    private readonly ICurrentUser currentUser;

    public IdentityService(ITokenGenerator tokenGenerator,
                           UserManager<User> userManager,
                           ICurrentUser currentUser)
    {
        this.tokenGenerator = tokenGenerator;
        this.userManager = userManager;
        this.currentUser = currentUser;
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
            ?? throw new AuthenticationException("Invalid username or password!");

        var passwordValid = await this.userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
        {
            throw new AuthenticationException("Invalid username or password!");
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

    public async Task<(AuthResponse, bool)> LoginOrRegisterExternalAsync(ExternalLoginInfo loginInfo)
    {
        bool isRegister = false;
        var user = await this.userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);

        if (user == null)
        {
            var email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                throw new ExternalLoginProviderException("Google", "Email not found in external login info.");
            }

            user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                };

                var createResult = await this.userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    throw new ExternalLoginProviderException("Google", "Failed to create user.");
                }
                isRegister = true;
            }

            var addLoginResult = await this.userManager.AddLoginAsync(user, loginInfo);
            if (!addLoginResult.Succeeded)
            {
                throw new ExternalLoginProviderException("Google", "Failed to associate external login.");
            }

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
        return (new AuthResponse
        {
            IsAdmin = isAdmin,
            UserId = user.Id,
            UserName = user.UserName ?? "",
            AccessToken = accessToken
        }, isRegister);
    }
    public async Task<AuthResponse> RegisterUserAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new AuthenticationException("Username is required!");
        }

        if (await this.userManager.FindByNameAsync(username) is not null)
        {
            throw new AuthenticationException("Username already exists!");
        }

        var user = new User { UserName = username };
        var result = await this.userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            throw new AuthenticationException("Failed to create user account!");
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


    public async Task<UserInfoForOrderModel> GetUserWithAdditionalInfo()
    {
        var userId = this.currentUser.UserId;
        if (userId == null)
        {
            throw new NotFoundException("User", "null");
        }

        var user = await this.userManager.Users
            .Include(x => x.Addresses)
            .ThenInclude(x => x.City)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("User", "null");
        }

        var userInfo = new UserInfoForOrderModel()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            TotalGreenPoints = user.TotalGreenPoints,
            FullAddress = user.Addresses.LastOrDefault() != null ? user.Addresses.LastOrDefault()!.FullAddress : null,
        };

        return userInfo;
    }

    public async Task ChangeUserInfoAsync(ChangeUserModel model)
    {
        var userId = this.currentUser.UserId;
        if (userId == null)
        {
            throw new NotFoundException("User");
        }
        var user = await this.userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User");
        }
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.TotalGreenPoints -= model.GreenPoints;

        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }
    }

    public async Task<GetUserInfoModel> GetUserInfoAsync()
    {
        var userId = this.currentUser.UserId;
        if (userId == null)
        {
            throw new NotFoundException("User", "null");
        }

        var user = await this.userManager.Users
            .Include(x => x.Addresses)
            .ThenInclude(x => x.City)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("User", "null");
        }

        var userInfo = new GetUserInfoModel()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            TotalGreenPoints = user.TotalGreenPoints,
            Addresses = user.Addresses.Select(x => new UserInfoAddress()
            {
                CityName = x.City.Name,
                Street = x.Street,
                Number = x.Number,
            })
        };

        return userInfo;
    }

    public async Task UpdateGreenPoints(string userId, int greenPoints)
    {
        var user = await this.userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("User", "null");
        }

        user.TotalGreenPoints += greenPoints;
        var result = await this.userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }
    }
}
