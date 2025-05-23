using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder
            .HasKey(x => new { x.OrderId, x.ProductId });

        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.OrderId);

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.ProductId);

        builder
            .HasOne(x => x.BaseCategory)
            .WithMany(x => x.OrderDetails)
            .HasForeignKey(x => x.BaseCategoryId);

        builder
            .Property(x => x.PricePerUnit)
            .HasColumnType("decimal(8,2)");

        builder
            .Property(x => x.TotalPrice)
            .HasColumnType("decimal(8,2)");
    }
}
