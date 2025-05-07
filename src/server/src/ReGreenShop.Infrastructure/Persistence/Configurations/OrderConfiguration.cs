using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        //Store status enum as string in the database for better readability
        builder
            .Property(x => x.Status)
            .HasConversion<string>();

        builder
            .HasOne<User>()
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId);

        builder
            .Property(x => x.InvoiceUrl)
            .HasMaxLength(MaxLengthLongName);

        builder
            .HasOne(x => x.DeliveryPrice)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.DeliveryPriceId);

        builder
            .HasOne(x => x.Address)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.AddressId);

        builder
             .HasOne(x => x.Payment)
             .WithOne(x => x.Order)
             .HasForeignKey<Order>(x => x.PaymentId);

        builder
            .HasOne(x => x.DiscountVoucher)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.DiscountVoucherId);
    }
}
