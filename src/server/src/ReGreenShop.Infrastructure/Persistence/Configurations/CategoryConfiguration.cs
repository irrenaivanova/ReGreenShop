using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(50);

        builder
            .HasOne(x => x.ParentCategory)
            .WithOne()
            .HasForeignKey<Category>(x => x.ParentCategoryId);

        builder
            .HasOne(x => x.Image)
            .WithOne(x => x.Category)
            .HasForeignKey<Category>(x => x.ImageId);
    }
}
