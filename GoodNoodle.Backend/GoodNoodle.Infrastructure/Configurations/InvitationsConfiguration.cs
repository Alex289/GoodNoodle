using GoodNoodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodNoodle.Infrastructure.Configurations;
public class InvitationsConfiguration : IEntityTypeConfiguration<Invitations>
{
    public void Configure(EntityTypeBuilder<Invitations> builder)
    {
        builder.Property(x => x.Role).IsRequired();
    }
}
