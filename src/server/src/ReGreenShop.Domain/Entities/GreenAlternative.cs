using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class GreenAlternative : BaseDeletableModel<int>
{
    public GreenAlternative()
    {
        this.OrderGreenAlternativeDetails = new HashSet<OrderGreenAlternativeDetail>();
    }

    public string Name { get; set; } = string.Empty;

    public int RewardPoints { get; set; }

    public IEnumerable<OrderGreenAlternativeDetail> OrderGreenAlternativeDetails { get; set; }
}
