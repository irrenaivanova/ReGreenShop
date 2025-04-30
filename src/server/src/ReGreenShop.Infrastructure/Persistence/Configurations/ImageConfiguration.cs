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
    }
}
