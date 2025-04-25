using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;

public class ContactForm : BaseModel<int>
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
