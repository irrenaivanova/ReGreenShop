using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .HasOne<User>()
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId);

        builder
            .Property(x => x.Text)
            .HasMaxLength(500);
    }
}
