using GoodNoodle.Api;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Infrastructure.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.IntegrationTests.Infrastructure;

public class GoodNoodleFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IWebHostBuilder CreateWebHostBuilder()
    {
        return WebHost
            .CreateDefaultBuilder()
            .UseStartup<Startup>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GoodNoodleContext>));
            services.Remove(descriptor);

            services.AddDbContext<GoodNoodleContext>(opt =>
            {
                opt.UseLazyLoadingProxies();
                opt.UseInMemoryDatabase("MemoryGoodNoodleDb");
            });

            ServiceProvider sp = services.BuildServiceProvider();

            using (IServiceScope scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var goodNoodleContext = scopedServices.GetRequiredService<GoodNoodleContext>();

                goodNoodleContext.Database.EnsureCreated();

                if (!goodNoodleContext.NoodleUser.Any(x => x.Id == Guid.Parse("5D3F674E-BA9B-48EA-9C34-861EA84E7B44")))
                {
                    var noodleUser = new NoodleUser(Guid.Parse("5D3F674E-BA9B-48EA-9C34-861EA84E7B44"))
                    {
                        FullName = "Max Mustermann",
                        Email = "max@mustermann.com",
                        Role = UserRole.Admin,
                        Status = UserStatus.Accepted,
                        // Password1#
                        Password = "$2a$12$SuKIMlg7qkFZcQzk8jkHtegMnfqO7v9JB9RArpRo6tuu9/6DQ.X.C"
                    };

                    goodNoodleContext.NoodleUser.Add(noodleUser);
                    goodNoodleContext.SaveChanges();
                }
            }
        });
    }
}
