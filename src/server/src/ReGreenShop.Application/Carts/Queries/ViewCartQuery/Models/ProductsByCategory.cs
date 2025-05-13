namespace ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery.Models;
public class ProductsByCategory
{
    public ProductsByCategory()
    {
        this.Products = new List<ProductInCartModel>();
    }

    public int Id { get; set; }

    public int CategoryName { get; set; }

    public IList<ProductInCartModel> Products { get; set; }
}
