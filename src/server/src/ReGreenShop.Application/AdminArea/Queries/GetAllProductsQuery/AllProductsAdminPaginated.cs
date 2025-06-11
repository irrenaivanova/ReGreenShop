namespace ReGreenShop.Application.AdminArea.Queries.GetAllProductsQuery;
public class AllProductsAdminPaginated
{
    public int TotalPages { get; set; }

    public IEnumerable<AdminProductInListModel> Products { get; set; } = new List<AdminProductInListModel>();
}
