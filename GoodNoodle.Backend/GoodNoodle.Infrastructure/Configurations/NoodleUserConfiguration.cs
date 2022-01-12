using GoodNoodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodNoodle.Infrastructure.Configurations;

public class NoodleUserConfiguration : IEntityTypeConfiguration<NoodleUser>
{
    public void Configure(EntityTypeBuilder<NoodleUser> builder)
    {
        builder.Property(x => x.Email).HasMaxLength(254).IsRequired();
        builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Password).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Status).IsRequired();
    }
}
