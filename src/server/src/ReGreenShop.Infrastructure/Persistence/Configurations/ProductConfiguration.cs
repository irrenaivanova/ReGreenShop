using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Ensures UserLikes are only included if the associated Property is not soft-deleted
        //modelBuilder.Entity<UserLike>()
        //    .HasQueryFilter(ul => !ul.Property.IsDeleted);

        throw new NotImplementedException();
    }
}
