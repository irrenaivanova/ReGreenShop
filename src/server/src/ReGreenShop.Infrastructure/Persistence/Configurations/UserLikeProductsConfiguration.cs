using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class UserLikeProductsConfiguration : IEntityTypeConfiguration<UserLikeProduct>
{
    public void Configure(EntityTypeBuilder<UserLikeProduct> builder)
    {
        builder
            .HasKey(x => new { x.ProductId, x.UserId });

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.UserLikes)
            .HasForeignKey(x => x.ProductId);

        builder
            .HasOne<User>()
            .WithMany(x => x.UserLikeProducts)
            .HasForeignKey(x => x.UserId);

        // Ensures UserLikes are only included if the associated Product is not soft-deleted
        builder
        .HasQueryFilter(x => !x.Product.IsDeleted);
    }
}
