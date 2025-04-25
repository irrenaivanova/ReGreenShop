using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Label : BaseDeletableModel<int>
{
    public Label()
    {
        LabelProducts = new HashSet<LabelProduct>();
    }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<LabelProduct> LabelProducts { get; set; }
}
