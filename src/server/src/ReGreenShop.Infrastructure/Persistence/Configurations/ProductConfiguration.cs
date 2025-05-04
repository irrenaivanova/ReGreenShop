using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(MaxLengthShortName);

        builder
            .Property(x => x.Description)
            .HasMaxLength(500);

        builder
            .Property(x => x.ProductCode)
            .HasMaxLength(MaxLengthShortName);

        builder
             .Property(x => x.Brand)
             .HasMaxLength(MaxLengthShortName);

        builder
              .Property(x => x.Origin)
              .HasMaxLength(MaxLengthShortName);

        builder
              .Property(x => x.Packaging)
              .HasMaxLength(MaxLengthShortName);

        builder
              .Property(x => x.OriginalUrl)
              .HasMaxLength(MaxLengthLongName);

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
