using Microsoft.AspNetCore.Identity;
using ReGreenShop.Domain.common;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Identity;
public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public virtual required ICollection<Order> Orders { get; set; }

    public virtual required ICollection<IdentityUserRole<string>> Roles { get; set; }

    public virtual required ICollection<IdentityUserClaim<string>> Claims { get; set; }

    public virtual required ICollection<IdentityUserLogin<string>> Logins { get; set; }

}
