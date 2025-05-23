namespace ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery.Models;
public class CartModel
{
    public CartModel()
    {
        ProductsByCategories = new List<ProductsByCategory>();
    }
    public IList<ProductsByCategory> ProductsByCategories { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal DeliveryPriceProducts { get; set; }

    public string? DeliveryMessage { get; set; }
}
