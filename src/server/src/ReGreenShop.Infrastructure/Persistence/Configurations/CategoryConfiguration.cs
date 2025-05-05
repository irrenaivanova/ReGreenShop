using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .Property(x => x.NameInBulgarian)
            .HasMaxLength(MaxLengthShortName);

        builder
            .Property(x => x.NameInEnglish)
            .HasMaxLength(MaxLengthShortName);

        builder
            .HasOne(x => x.ParentCategory)
            .WithOne()
            .HasForeignKey<Category>(x => x.ParentCategoryId);

        builder
            .HasOne(x => x.Image)
            .WithOne()
            .HasForeignKey<Category>(x => x.ImageId);
    }
}
