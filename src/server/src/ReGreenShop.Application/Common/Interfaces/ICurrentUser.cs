using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface ICurrentUser : IScopedService
{
    string? UserId { get; }
}
