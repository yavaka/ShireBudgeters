using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShireBudgeters.BL.Services.Identity;
using ShireBudgeters.DA.Configurations;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.BL.Configurations;

public static class BusinessLogicConfigurations
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessServices(configuration)
            .AddAuthNAndAuthZ()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddAuthNAndAuthZ(this IServiceCollection services)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddIdentityCookies();
        
        services.AddIdentityCore<UserModel>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
        })
            .AddEntityFrameworkStores<ShireBudgetersDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }
    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<IIdentityService, IdentityService>();
}
