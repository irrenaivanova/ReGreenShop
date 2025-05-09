namespace ReGreenShop.Application.Categories.Queries.GetSubCategoriesByRootCategoryId.Models;
public class SubCategoryModel
{
    public SubCategoryModel()
    {
        SubSubCategories = new List<SubSubCategoryModel>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IList<SubSubCategoryModel> SubSubCategories { get; set; }
}
