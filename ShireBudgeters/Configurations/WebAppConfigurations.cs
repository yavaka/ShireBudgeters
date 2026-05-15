using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Radzen;
using ShireBudgeters.BL.Configurations;

namespace ShireBudgeters.Configurations;

public static class WebAppConfigurations
{
    public static IServiceCollection AddWebAppServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // HtmlEditor / large paste: default SignalR receive cap (~32 KB) drops the Blazor circuit.
        services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddHubOptions(options =>
            {
                options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
            });

        // Radzen
        services.AddRadzenComponents();

        // Business Logic
        services.AddBusinessLogicServices(configuration);

        return services;
    }
}