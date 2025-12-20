using Radzen;
using ShireBudgeters.BL.Configurations;

namespace ShireBudgeters.Configurations;

public static class WebAppConfigurations
{
    public static IServiceCollection AddWebAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Radzen
        services.AddRadzenComponents();

        // Business Logic
        services.AddBusinessLogicServices(configuration);

        return services;
    }
}
