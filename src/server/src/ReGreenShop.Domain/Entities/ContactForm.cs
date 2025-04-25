using ReGreenShop.Domain.common;

namespace ReGreenShop.Domain.Entities;
public class ContactForm : BaseModel<int>
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}
