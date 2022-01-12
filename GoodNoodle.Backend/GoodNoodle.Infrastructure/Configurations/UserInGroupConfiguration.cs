using GoodNoodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodNoodle.Infrastructure.Configurations;

public class UserInGroupConfiguration : IEntityTypeConfiguration<UserInGroup>
{
    public void Configure(EntityTypeBuilder<UserInGroup> builder)
    {
        builder.Property(x => x.Role).IsRequired();
        builder.HasOne(x => x.NoodleGroup).WithMany(g => g.UserInGroups).HasForeignKey(p => p.NoodleGroupId);
        builder.HasOne(x => x.NoodleUser).WithMany(g => g.UserInGroups).HasForeignKey(p => p.NoodleUserId);
    }
}
