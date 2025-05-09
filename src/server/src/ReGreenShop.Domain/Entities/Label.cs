using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Label : BaseDeletableModel<int>
{
    public Label()
    {
        LabelProducts = new List<LabelProduct>();
    }

    public string Name { get; set; } = string.Empty;

    public IList<LabelProduct> LabelProducts { get; set; }
}
