using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class DeliveryPriceConfiguration : IEntityTypeConfiguration<DeliveryPrice>
{
    public void Configure(EntityTypeBuilder<DeliveryPrice> builder)
    {
        builder
            .Property(x => x.Price)
            .HasColumnType("decimal(8,2)");

        builder
            .Property(x => x.MinPriceOrder)
            .HasColumnType("decimal(8,2)");

        builder
            .Property(x => x.MaxPriceOrder)
            .HasColumnType("decimal(8,2)");
    }
}
