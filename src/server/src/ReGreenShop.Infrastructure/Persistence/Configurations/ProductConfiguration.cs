using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(50);

        builder
            .Property(x => x.Description)
            .HasMaxLength(500);

        builder
            .Property(x => x.ProductCode)
            .HasMaxLength(50);

        builder
             .Property(x => x.Brand)
             .HasMaxLength(50);

        builder
              .Property(x => x.Origin)
              .HasMaxLength(50);

        builder
            .HasOne(x => x.Image)
            .WithOne()
            .HasForeignKey<Product>(x => x.ImageId);

        builder
            .Property(x => x.Price)
            .HasColumnType("decimal(8,2)");

        builder
            .Property(x => x.Weight)
            .HasColumnType("decimal(8,2)");
    }
}
