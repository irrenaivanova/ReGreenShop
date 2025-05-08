using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Services;
public class CartService : ICart
{
    private readonly ICurrentUser user;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly IData data;
    private readonly UserManager<User> userManager;

    public CartService(ICurrentUser user,
                    IHttpContextAccessor contextAccessor,
                    IData data,
                    UserManager<User> userManager)
    {
        this.user = user;
        this.contextAccessor = contextAccessor;
        this.data = data;
        this.userManager = userManager;
    }

    public async Task<string> GetCartId()
    {
        // Exception of context is null ?
        var context = this.contextAccessor.HttpContext;
        var userId = this.user.UserId;

        if (userId == null)
        {
            if (context!.Session.GetString(SessionId) == null)
            {
                var newSessionValue = Guid.NewGuid().ToString();
                context.Session.SetString(SessionId, newSessionValue);
            }

            string sessionValue = context!.Session.GetString(SessionId)!;
            var cart = new Cart()
            {
                Session = sessionValue
            };
            this.data.Carts.Add(cart);
            await this.data.SaveChangesAsync();
            return cart.Id;
        }

        return this.data.Carts.FirstOrDefault(x => x.UserId == userId)!.Id;
    }

    // GetCartItems - in query ?

    public async Task<int> GetCountProductsInCart()
    {
        var cartId = await GetCartId();
        return this.data.Carts.FirstOrDefault(x => x.Id == cartId)!.CartItems.Count();
    }

    public async Task MergeCartIfAnyAsync(string userId)
    {
        // exception if context null
        var context = this.contextAccessor.HttpContext;
        string sessionValue = context!.Session.GetString(SessionId)!;
        if (sessionValue!=null)
        {
            var sessionIdCart = this.data.Carts.FirstOrDefault(x => x.Session == sessionValue);
            if (sessionIdCart != null)
            {
                var userCartId = this.data.Carts.FirstOrDefault(x => x.UserId == userId)!.Id;
                foreach (var item in sessionIdCart.CartItems)
                {
                    item.CartId = userCartId;
                }
                await this.data.SaveChangesAsync();
            }
        }  
    }

    public async Task<string> CreateCartAsync(string userId)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User", userId);
        }
        var cart = new Cart()
        {
            UserId = userId
        };
        this.data.Carts.Add(cart);
        user!.CartId = cart.Id;
        await this.data.SaveChangesAsync();
        return cart.Id;
    }
}
