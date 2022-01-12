using GoodNoodle.AdminTool.Provider;
using GoodNoodle.AdminTool.Services;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static GoodNoodle.Domain.Enums;
using BC = BCrypt.Net.BCrypt;

namespace GoodNoodle.AdminTool;

public class Program
{
    public static ServiceProvider Services { get; private set; }
    public static IConfiguration Configuration { get; private set; }

    public static async Task Main(string[] args)
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection = ConfigureServices(serviceCollection);
        Services = serviceCollection.BuildServiceProvider();

        await RunAsync();
    }

    private static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<GoodNoodleContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IGoodNoodleProvider, GoodNoodleProvider>();
        services.AddSingleton<IAdminService, AdminService>();
        return services;
    }

    public static async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("AdminTool");

        Console.WriteLine("Fullname: ");
        string fullName = Console.ReadLine() ?? "Admin";

        Console.WriteLine("Email: ");
        string email = Console.ReadLine() ?? "admin@goodnoodle.com";

        Console.WriteLine("Password: ");
        string password = Console.ReadLine() ?? "Password123#";

        Console.Clear();

        var hashedPassword = BC.HashPassword(password);
        var id = Guid.NewGuid();

        var admin = new NoodleUser(id)
        {
            FullName = fullName,
            Email = email,
            Password = hashedPassword,
            Status = UserStatus.Accepted,
            Role = UserRole.Admin,
        };

        IAdminService adminService = Services.GetService<IAdminService>();
        await adminService.CreateAdmin(admin);

        Console.WriteLine("Successfully created admin user:\n");

        var createdAdmin = await adminService.GetAdmin(id);

        Console.WriteLine("Id:                  " + createdAdmin.Id);
        Console.WriteLine("FullName:            " + createdAdmin.FullName);
        Console.WriteLine("Email:               " + createdAdmin.Email);
        Console.WriteLine("Password:            " + password);
        Console.WriteLine("HashedPassword:      " + createdAdmin.Password);
        Console.WriteLine("Status:              " + createdAdmin.Status);
        Console.WriteLine("Role:                " + createdAdmin.Role);
    }
}
