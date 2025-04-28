using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(50);

        builder
            .Property(x => x.LocalPath)
            .HasMaxLength(100);

        builder
            .Property(x => x.BlobPath)
            .HasMaxLength(100);

        builder
            .Property(x => x.Format)
            .HasMaxLength(10);

        builder
            .HasOne(x => x.Product)
            .WithOne(x => x.Image)
            .HasForeignKey<Image>(x => x.ProductId);


        builder
            .HasOne(x => x.Category)
            .WithOne(x => x.Image)
            .HasForeignKey<Image>(x => x.CategoryId);
    }
}
