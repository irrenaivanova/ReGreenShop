namespace ReGreenShop.Domain.Entities;
public class UserLikeProduct
{
    public string UserId { get; set; } = string.Empty;

    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;
}
