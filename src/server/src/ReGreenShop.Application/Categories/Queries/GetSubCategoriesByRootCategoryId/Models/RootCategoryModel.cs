namespace ReGreenShop.Application.Categories.Queries.GetSubCategoriesByRootCategoryId.Models;
public class RootCategoryModel
{
    public RootCategoryModel()
    {
        this.SubCategories = new List<SubCategoryModel>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IList<SubCategoryModel> SubCategories { get; set; }
}
