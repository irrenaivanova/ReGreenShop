using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class LabelProductConfiguration : IEntityTypeConfiguration<LabelProduct>
{
    public void Configure(EntityTypeBuilder<LabelProduct> builder)
    {
        builder
            .HasKey(x => new { x.ProductId, x.LabelId });

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.LabelProducts)
            .HasForeignKey(x => x.ProductId);

        builder
            .HasOne(x => x.Label)
            .WithMany(x => x.LabelProducts)
            .HasForeignKey(x => x.LabelId);

        builder
            .HasQueryFilter(x => !x.Label.IsDeleted);
    }
}
