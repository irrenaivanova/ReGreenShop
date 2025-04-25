namespace ReGreenShop.Domain.common;
public interface IAuditableEntity
{
    DateTime CreatedOn { get; set; }
    DateTime? ModifiedOn { get; set; }
}
