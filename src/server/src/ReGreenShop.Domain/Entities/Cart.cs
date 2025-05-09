using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Cart : BaseDeletableModel<string>
{
    public Cart()
    {
        Id = Guid.NewGuid().ToString();
        CartItems = new List<CartItem>();
    }

    public string? UserId { get; set; }

    public string? Session { get; set; }

    public IList<CartItem> CartItems { get; set; }
}
