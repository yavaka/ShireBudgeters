using Microsoft.AspNetCore.Identity;
using Radzen;
using ShireBudgeters.BL.Configurations;
using ShireBudgeters.Common.Common.Constants;

namespace ShireBudgeters.Configurations;

public static class WebAppConfigurations
{
    public static IServiceCollection AddWebAppServices(
        this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Radzen
        services.AddRadzenComponents();

        // Business Logic
        services.AddBusinessLogicServices(configuration, environment.IsProduction());

        return services;
    }
}