namespace ReGreenShop.Domain.common;
public class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
}
