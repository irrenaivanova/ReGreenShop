using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class ContactFormConfiguration : IEntityTypeConfiguration<ContactForm>
{
    public void Configure(EntityTypeBuilder<ContactForm> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(MaxLengthShortName);

        builder
            .Property(x => x.Email)
            .HasMaxLength(MaxLengthShortName);

        builder
            .Property(x => x.Title)
            .HasMaxLength(MaxLengthShortName);

        builder
            .Property(x => x.Content)
            .HasMaxLength(1000);
    }
}
