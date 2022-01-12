using GoodNoodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodNoodle.Infrastructure.Configurations;

public class StarConfiguration : IEntityTypeConfiguration<Star>
{
    public void Configure(EntityTypeBuilder<Star> builder)
    {
        builder.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
    }
}
