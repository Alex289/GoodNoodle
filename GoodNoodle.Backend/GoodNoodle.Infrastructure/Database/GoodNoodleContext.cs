using GoodNoodle.Domain.Entities;
using GoodNoodle.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GoodNoodle.Infrastructure.Database;

public class GoodNoodleContext : DbContext
{
    public GoodNoodleContext(DbContextOptions<GoodNoodleContext> options)
        : base(options)
    {
    }

    public DbSet<NoodleGroup> NoodleGroup { get; set; }
    public DbSet<NoodleUser> NoodleUser { get; set; }
    public DbSet<UserInGroup> UserInGroup { get; set; }
    public DbSet<Star> Star { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NoodleGroupConfiguration());
        modelBuilder.ApplyConfiguration(new NoodleUserConfiguration());
        modelBuilder.ApplyConfiguration(new StarConfiguration());
        modelBuilder.ApplyConfiguration(new UserInGroupConfiguration());
        modelBuilder.ApplyConfiguration(new InvitationsConfiguration());
    }
}
