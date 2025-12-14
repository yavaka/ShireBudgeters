using Radzen;

namespace ShireBudgeters.Configurations;

public static class WebAppConfigurations
{
    public static IServiceCollection AddWebAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddRadzenComponents();

        // Add your web app services here
        return services;
    }
}
