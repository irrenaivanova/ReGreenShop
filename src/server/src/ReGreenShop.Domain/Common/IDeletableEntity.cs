namespace ReGreenShop.Domain.common;
public interface IDeletableEntity
{
    bool IsDeleted { get; set; }
    DateTime? DeletedOn { get; set; }
}
