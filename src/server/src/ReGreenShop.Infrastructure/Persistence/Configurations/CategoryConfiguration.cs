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
            .HasMaxLength(MaxLengthLongName);

        builder
            .Property(x => x.NameInEnglish)
            .HasMaxLength(MaxLengthLongName);

        builder
             .HasOne(c => c.ParentCategory)
             .WithMany(c => c.ChildCategories)
             .HasForeignKey(c => c.ParentCategoryId)
             .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Image)
            .WithOne()
            .HasForeignKey<Category>(x => x.ImageId);
    }
}
