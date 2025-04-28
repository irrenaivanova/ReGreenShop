using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder
            .HasOne<User>()
            .WithMany(u => u.Addresses)
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.City)
            .WithMany(u => u.Addresses)
            .HasForeignKey(x => x.CityId);

        builder
            .Property(x => x.Street)
            .HasMaxLength(100);
    }
}
