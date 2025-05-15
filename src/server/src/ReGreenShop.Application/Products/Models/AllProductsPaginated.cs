namespace ReGreenShop.Application.Products.Models;
public class AllProductsPaginated : PageInfo
{
    public AllProductsPaginated()
    {
        Products = new List<ProductInList>();
    }

    public IEnumerable<ProductInList> Products { get; set; }
}
