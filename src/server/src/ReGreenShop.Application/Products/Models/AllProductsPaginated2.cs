namespace ReGreenShop.Application.Products.Models;
public class AllProductsPaginated2 : PageInfo
{
    public AllProductsPaginated2()
    {
        Products = new List<ProductInList2>();
    }

    public IEnumerable<ProductInList2> Products { get; set; }
}
