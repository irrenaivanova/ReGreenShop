using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReGreenShop.Domain.Entities;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Persistence.Configurations;
public class GreenAlternativeConfiguration : IEntityTypeConfiguration<GreenAlternative>
{
    public void Configure(EntityTypeBuilder<GreenAlternative> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(MaxLengthLongName);
    }
}
