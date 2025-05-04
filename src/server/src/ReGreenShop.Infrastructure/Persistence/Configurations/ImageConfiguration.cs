using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(MaxLengthShortName);

        builder
            .Property(x => x.LocalPath)
            .HasMaxLength(MaxLengthLongName);

        builder
            .Property(x => x.BlobPath)
            .HasMaxLength(MaxLengthLongName);
    }
}
