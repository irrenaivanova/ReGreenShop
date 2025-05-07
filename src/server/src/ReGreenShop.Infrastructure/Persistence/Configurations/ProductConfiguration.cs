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
            .HasMaxLength(MaxLengthLongName);

        builder
            .Property(x => x.Description)
            .HasMaxLength(1000);

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
              .HasMaxLength(200);

        builder
            .HasOne(x => x.Image)
            .WithOne()
            .HasForeignKey<Product>(x => x.ImageId);

        builder
            .Property(x => x.Price)
            .HasColumnType("decimal(8,2)");

    }
}
