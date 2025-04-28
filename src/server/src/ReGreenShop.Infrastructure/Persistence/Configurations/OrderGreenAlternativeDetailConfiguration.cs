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
    }
}
