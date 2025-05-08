using ReGreenShop.Application.Common.Services;

namespace ReGreenShop.Application.Common.Interfaces;
public interface ICart : IService
{
    Task<string> GetCartId();

    Task<int> GetCountProductsInCart();

    Task MergeCartIfAnyAsync(string userId);

    Task<string> CreateCartAsync(string userId);
}
