using Microsoft.AspNetCore.Identity;
using ReGreenShop.Domain.common;

namespace ReGreenShop.Infrastructure.Persistence.Identity;
public class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
{
    public ApplicationRole(string name)
           : base(name)
    {
        Id = Guid.NewGuid().ToString();
    }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
