namespace ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery.Models;
public class ProductsByCategory
{
    public ProductsByCategory()
    {
        Products = new List<ProductInCartModel>();
    }

    public int Id { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public IList<ProductInCartModel> Products { get; set; }
}
