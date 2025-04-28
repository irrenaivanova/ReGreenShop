using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class GreenAlternativeConfiguration : IEntityTypeConfiguration<GreenAlternative>
{
    public void Configure(EntityTypeBuilder<GreenAlternative> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(50);
    }
}
