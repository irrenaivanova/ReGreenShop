namespace ReGreenShop.Application.Common.Interfaces;
public interface ICart
{
    Task<string> GetCartId();

    Task<int> GetCountProductsInCart();

    Task MergeCartIfAnyAsync();

    Task<string> CreateCartAsync(string userId);
}
