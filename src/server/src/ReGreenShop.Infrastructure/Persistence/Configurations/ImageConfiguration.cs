using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class ImageConfiguration
{

    //// One-to-one relationship between Image and Product
    //builder.Entity<Image>()
    //    .HasOne(i => i.Product)
    //    .WithOne(p => p.Image)
    //    .HasForeignKey<Image>(i => i.ProductId)
    //    .IsRequired(false);  // Make the foreign key optional

    //// One-to-one relationship between Image and Category
    //builder.Entity<Image>()
    //    .HasOne(i => i.Category)
    //    .WithOne(c => c.Image)
    //    .HasForeignKey<Image>(i => i.CategoryId)
    //    .IsRequired(false);  // Make the foreign key optional
}
