using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class Notification : BaseModel<int>
{
    public string UserId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public bool IsRead { get; set; }
}
