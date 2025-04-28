using Microsoft.AspNetCore.Identity;
using ReGreenShop.Domain.common;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Identity;
public class User : IdentityUser, IAuditableEntity, IDeletableEntity
{
    public User()
    {
       this.Id = Guid.NewGuid().ToString();
       this.TotalGreenPoints = 0;
       this.Notifications = new HashSet<Notification>();
       this.Orders = new HashSet<Order>();
       this.Addresses = new HashSet<Address>();
       this.UserLikeProducts = new HashSet<UserLikeProduct>();
    }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int TotalGreenPoints { get; set; }

    public Cart Cart { get; set; } = default!;

    public string CartId { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public ICollection<Notification> Notifications { get; set; }

    public ICollection<Address> Addresses { get; set; }

    public ICollection<Order> Orders { get; set; }

    public ICollection<UserLikeProduct> UserLikeProducts { get; set; }
}
