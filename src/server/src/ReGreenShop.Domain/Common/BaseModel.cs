namespace ReGreenShop.Domain.common;
public class BaseModel<TKey> : IAuditInfo
{
    public TKey Id { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
