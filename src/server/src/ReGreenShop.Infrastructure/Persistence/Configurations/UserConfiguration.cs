using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Infrastructure.Persistence.Identity;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasOne(x => x.Cart)
            .WithOne()
            .HasForeignKey<User>(x => x.CartId);

        builder
            .Property(x => x.FirstName)
            .HasMaxLength(50);

        builder
            .Property(x => x.LastName)
            .HasMaxLength(50);

        // Setting maximum length for these properties because they are default NVARCHAR(MAX) if not specified

        builder.Property(b => b.Email)
            .HasMaxLength(128);

        builder.Property(b => b.NormalizedEmail)
            .HasMaxLength(128);

        builder.Property(b => b.UserName)
            .HasMaxLength(128);

        builder.Property(b => b.NormalizedUserName)
            .HasMaxLength(128);
    }
}
