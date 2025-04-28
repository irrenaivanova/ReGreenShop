using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder
            .HasKey(x => new {x.ProductId, x.CategoryId});

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductCategories)
            .HasForeignKey(x => x.ProductId);

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.ProductCategories)
            .HasForeignKey(x => x.CategoryId);

        // Ensures ProductCategories only included if the associated Product/Category is not soft-deleted
        builder
            .HasQueryFilter(x => !x.Category.IsDeleted);

        builder
            .HasQueryFilter(x => !x.Category.IsDeleted);
    }
}
