using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.GreenAlternatives.GetAllGreenAlternativesQuery;
public class GreenAlternativesModel : IMapFrom<GreenAlternative>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
