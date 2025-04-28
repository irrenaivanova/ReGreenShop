using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class DiscountVoucherConfiguration : IEntityTypeConfiguration<DiscountVoucher>
{
    public void Configure(EntityTypeBuilder<DiscountVoucher> builder)
    {
        builder
            .HasOne(x => x.Order)
            .WithOne(x => x.DiscountVoucher)
            .HasForeignKey<DiscountVoucher>(x => x.OrderId);
    }
}
