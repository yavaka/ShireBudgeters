using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShireBudgeters.DA.Configurations.Database;

namespace ShireBudgeters.DA.Configurations;

/// <summary>
/// Configuration for the data access services.
/// </summary>
public static class DataAccessConfigurations
{
    /// <summary>
    /// Adds the data access services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDAOptions(configuration);

        // Configure the DbContext with SQL Server provider
        services.AddDbContext<ShireBudgetersDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(ShireBudgetersDbContext).Assembly.FullName));

            if (configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                // Enable sensitive data logging only in development
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });


        return services;
    }

    /// <summary>
    /// Adds the options for the admin settings.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection AddDAOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
