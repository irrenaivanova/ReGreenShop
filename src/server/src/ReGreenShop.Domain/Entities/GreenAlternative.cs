using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class GreenAlternative : BaseDeletableModel<int>
{
    public GreenAlternative()
    {
        OrderGreenAlternativeDetails = new List<OrderGreenAlternativeDetail>();
    }

    public string Name { get; set; } = string.Empty;

    public int RewardPoints { get; set; }

    public IList<OrderGreenAlternativeDetail> OrderGreenAlternativeDetails { get; set; }
}
