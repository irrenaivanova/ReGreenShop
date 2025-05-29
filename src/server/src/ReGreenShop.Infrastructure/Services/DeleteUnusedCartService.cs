using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Infrastructure.Persistence.Identity;

namespace ReGreenShop.Infrastructure.Services;
public class DeleteUnusedCartService : IDeleteUnusedCart
{
    private readonly IData data;
    private readonly UserManager<User> userManager;

    public DeleteUnusedCartService(IData data, UserManager<User> userManager)
    {
        this.data = data;
        this.userManager = userManager;
    }

    public async Task DeleteUnusedCartsDaily()
    {
        var usedCartIds = await this.userManager.Users
                .Where(x => x.CartId != null)
                .Select(x => x.CartId)
                .ToListAsync();
        var cutoffTime = DateTime.Now.AddHours(-24);

        var deletableCarts = await this.data.Carts.Where(x => x.UserId == null &&
                !usedCartIds.Contains(x.Id) && x.CreatedOn < cutoffTime)
                .ToListAsync();

        var deletableCartIds = deletableCarts.Select(c => c.Id).ToList();
        var cartItemsToDelete = await this.data.CartItems
            .Where(ci => deletableCartIds.Contains(ci.CartId))
            .ToListAsync();

        this.data.CartItems.RemoveRange(cartItemsToDelete);
        this.data.Carts.RemoveRange(deletableCarts);
        await this.data.SaveChangesAsync();
    }
}
