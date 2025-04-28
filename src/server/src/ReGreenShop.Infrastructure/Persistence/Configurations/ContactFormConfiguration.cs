using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class ContactFormConfiguration : IEntityTypeConfiguration<ContactForm>
{
    public void Configure(EntityTypeBuilder<ContactForm> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(50);

        builder
            .Property(x => x.Email)
            .HasMaxLength (50);

        builder
            .Property(x => x.Title)
            .HasMaxLength(50);

        builder
            .Property(x => x.Content)
            .HasMaxLength(1000);
    }
}
