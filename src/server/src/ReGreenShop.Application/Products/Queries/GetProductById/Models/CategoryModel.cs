using AutoMapper;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Products.Queries.GetProductById.Models;
public class CategoryModel : IMapFrom<ProductCategory>, IMapExplicitly
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public void CreateMappings(IProfileExpression configuration)
    {
        throw new NotImplementedException();
    }
}
