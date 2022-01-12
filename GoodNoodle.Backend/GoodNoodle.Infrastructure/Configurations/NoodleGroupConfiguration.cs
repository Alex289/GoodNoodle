using GoodNoodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodNoodle.Infrastructure.Configurations;

public class NoodleGroupConfiguration : IEntityTypeConfiguration<NoodleGroup>
{
    public void Configure(EntityTypeBuilder<NoodleGroup> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
    }
}
