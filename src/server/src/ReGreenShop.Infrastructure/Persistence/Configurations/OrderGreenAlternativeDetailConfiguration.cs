using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class OrderGreenAlternativeDetailConfiguration : IEntityTypeConfiguration<OrderGreenAlternativeDetail>
{
    public void Configure(EntityTypeBuilder<OrderGreenAlternativeDetail> builder)
    {
        builder
            .HasKey(x => new {x.OrderId, x.GreenAlternativeId});

        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderGreenAlternativeDetails)
            .HasForeignKey(x => x.OrderId);

        builder
            .HasOne(x => x.GreenAlternative)
            .WithMany(x=> x.OrderGreenAlternativeDetails)
            .HasForeignKey(x => x.GreenAlternativeId);
    }
}
