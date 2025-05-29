using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface IDeleteUnusedCart : IService
{
    Task DeleteUnusedCartsDaily();
}
