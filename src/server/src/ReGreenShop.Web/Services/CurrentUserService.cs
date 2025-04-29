using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Web.Services;
public class CurrentUserService : ICurrentUser
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
            => UserId = httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserId { get; }
}
