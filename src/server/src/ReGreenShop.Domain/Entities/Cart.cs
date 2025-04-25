using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Cart : BaseDeletableModel<string>
{
    public Cart()
    {
        this.Id = Guid.NewGuid().ToString();
        this.CartItems = new HashSet<CartItem>();
    }

    public string? UserId { get; set; }

    public string? SessionId { get; set; }

    public IEnumerable<CartItem> CartItems { get; set; }
}
