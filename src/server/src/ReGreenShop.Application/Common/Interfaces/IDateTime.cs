using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IDateTime : IService
{
    DateTime Now { get; }
}
