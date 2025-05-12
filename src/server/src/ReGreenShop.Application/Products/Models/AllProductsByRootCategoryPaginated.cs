namespace ReGreenShop.Application.Products.Models;
public class AllProductsByRootCategoryPaginated : PageInfo
{
    public AllProductsByRootCategoryPaginated()
    {
        this.Products = new List<ProductInList>();
    }

    public IEnumerable<ProductInList> Products { get; set; }
}
