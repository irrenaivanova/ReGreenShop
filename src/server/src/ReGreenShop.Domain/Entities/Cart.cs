using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;
public class Cart : BaseDeletableModel<string>
{
    public Cart()
    {
        CartItems = new HashSet<CartItem>();
    }

    public string? UserId { get; set; } = string.Empty;

    public string? SessionId { get; set; } = string.Empty;

    public IEnumerable<CartItem> CartItems { get; set; }
}
