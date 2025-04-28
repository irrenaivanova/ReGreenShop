using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder
            .HasOne<User>()
            .WithOne(x => x.Cart)
            .HasForeignKey<Cart>(x => x.UserId);

        builder
            .Property(x => x.Session)
            .HasMaxLength(36);
    }
}
