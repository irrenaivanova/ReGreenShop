using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface ICart : IService
{
    Task<string> GetCartIdAsync();

    Task<int> GetCountProductsInCartAsync();

    Task MergeCartIfAnyAsync(string userId);

    Task<string> CreateCartAsync(string userId);

    Task ClearCartAsync();

    Task<int> GetCountOfConcreteProductInCartAsync(int id);
}
