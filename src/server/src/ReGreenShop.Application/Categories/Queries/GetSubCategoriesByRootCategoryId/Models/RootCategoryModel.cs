namespace ReGreenShop.Application.Categories.Queries.GetSubCategoriesByRootCategoryId.Models;
public class RootCategoryModel
{
    public RootCategoryModel()
    {
        SubCategories = new List<SubCategoryModel>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? ImagePath { get; set; } 

    public IList<SubCategoryModel> SubCategories { get; set; }
}
