using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder
            .HasKey(x => new {x.CartId, x.ProductId});

        builder
            .HasOne(x => x.Cart)
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.CartId);

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.ProductId);

        builder
            .HasOne(x => x.BaseCategory)
            .WithMany(x => x.CartItems)
            .HasForeignKey(x => x.BaseCategoryId);

        builder
            .HasQueryFilter(x => !x.BaseCategory.IsDeleted);
    }
}
