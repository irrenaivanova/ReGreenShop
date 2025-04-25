using Microsoft.AspNetCore.Identity;
using ReGreenShop.Domain.common;

namespace ReGreenShop.Infrastructure.Persistence.Identity;
public class Role : IdentityRole, IAuditInfo, IDeletableEntity
{
    public Role(string name)
           : base(name)
    {
        Id = Guid.NewGuid().ToString();
    }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
