using GoodNoodle.Domain.Entities;
using System;
using System.Linq;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Infrastructure.Database;

public class DbInitializer
{
    public static void Initialize(GoodNoodleContext context)
    {
        context.Database.EnsureCreated();

        if (context.NoodleUser.Any())
        {
            return;
        }

        var admin = new NoodleUser(Guid.NewGuid())
        {
            FullName = "Admin",
            Email = "goodnoodle.noreply@gmail.com",
            // Password: !Password123#
            Password = "$2a$12$Blal/uiFIJdYsCLTMUik/egLbfg3XhbnxBC6Sb5IKz2ZYhiU/MzL2",
            Status = UserStatus.Accepted
        };

        context.NoodleUser.Add(admin);
        context.SaveChanges();
    }
}
