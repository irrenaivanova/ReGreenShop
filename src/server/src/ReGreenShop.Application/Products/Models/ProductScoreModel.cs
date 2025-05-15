using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Products.Models;
public class ProductScoreModel
{
    public required Product Product { get; set; }
    public int Score { get; set; }
}
