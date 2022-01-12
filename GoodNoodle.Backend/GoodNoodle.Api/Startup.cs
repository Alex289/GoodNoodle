using GoodNoodle.Application.Extension;
using GoodNoodle.Domain;
using GoodNoodle.Domain.Extensions;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Settings;
using GoodNoodle.Infrastructure;
using GoodNoodle.Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GoodNoodle.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<GoodNoodleContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

        services.AddRepositories();

        services.AddControllers();

        services.AddRequestHandlers();
        services.AddQueryHandlers();
        services.AddEventHandlers();

        services.AddMediatR(typeof(Startup));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddServices();

        var tokenSettings = new TokenSettings();
        Configuration.Bind("Jwt", tokenSettings);
        services.AddSingleton(tokenSettings);

        var mailSettings = new MailSettings();
        Configuration.Bind("MailSettings", mailSettings);
        services.AddSingleton(mailSettings);

        var hostingSettings = new HostingSettings()
        {
            Domain = Configuration["Hosting:Domain"]
        };
        services.AddSingleton(hostingSettings);


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        Configuration["Jwt:Secret"]))
            };
        });

        services.AddHttpContextAccessor();
        services.AddScoped<IUser, ApplicationUser>();

        // Domain events
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoodNoodle.Api", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoodNoodle.Api v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(Configuration["Hosting:Domain"]));

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
