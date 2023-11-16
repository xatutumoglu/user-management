using Microsoft.EntityFrameworkCore;
using NTTUserApp.Data.Entities;
using NTTUserApp.Service.Abstractions;
using NTTUserApp.Service.Implementations;

namespace NTTUserApp.Utils;
public static class Bootstraper
{
    public static IServiceCollection AddSqlDb(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<MyDbContext>(options => {
            options.UseSqlServer(configuration.GetConnectionString("MyDbContext"));
        });

        return services;
    }

    public static IServiceCollection AddServiceCollections(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }

}