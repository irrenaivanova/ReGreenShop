namespace ReGreenShop.Domain.common;
public interface IAuditInfo
{
    DateTime CreatedOn { get; set; }
    DateTime? ModifiedOn { get; set; }
}
