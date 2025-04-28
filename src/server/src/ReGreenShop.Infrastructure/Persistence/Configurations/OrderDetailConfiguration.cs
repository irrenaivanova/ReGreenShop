using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder
            .HasKey(x => new {x.OrderId, x.ProductId});

        builder
            .HasOne(x => x.BaseCategory)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.BaseCategoryId);
    }
}
