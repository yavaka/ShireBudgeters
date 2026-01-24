using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShireBudgeters.BL.Services.Category;
using ShireBudgeters.BL.Services.Identity;
using ShireBudgeters.BL.Services.LeadMagnet;
using ShireBudgeters.BL.Services.Post;
using ShireBudgeters.DA.Configurations;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;
using ShireBudgeters.DA.Repositories.Category;

namespace ShireBudgeters.BL.Configurations;

public static class BusinessLogicConfigurations
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccessServices(configuration)
            .AddAuthNAndAuthZ(configuration)
            .AddServices();

        return services;
    }

    private static IServiceCollection AddAuthNAndAuthZ(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        // Determine if we're in development from configuration
        var isDevelopment = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development";

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddIdentityCookies(options =>
            {
                // Configure cookie security for production
                var securePolicy = isDevelopment 
                    ? CookieSecurePolicy.SameAsRequest 
                    : CookieSecurePolicy.Always; // HTTPS only in production
                
                options.ApplicationCookie?.Configure(cookieOptions =>
                {
                    cookieOptions.Cookie.Name = "ShireBudgeters.Auth";
                    cookieOptions.Cookie.HttpOnly = true; // Prevent XSS attacks
                    cookieOptions.Cookie.SameSite = SameSiteMode.Strict; // CSRF protection
                    cookieOptions.Cookie.SecurePolicy = securePolicy;
                    cookieOptions.ExpireTimeSpan = TimeSpan.FromHours(8); // Session timeout
                    cookieOptions.SlidingExpiration = true; // Extend session on activity
                    cookieOptions.LoginPath = "/Account/Login";
                    cookieOptions.LogoutPath = "/Account/Logout";
                    cookieOptions.AccessDeniedPath = "/Account/AccessDenied";
                });

                options.ExternalCookie?.Configure(cookieOptions =>
                {
                    cookieOptions.Cookie.HttpOnly = true;
                    cookieOptions.Cookie.SameSite = SameSiteMode.Strict;
                    cookieOptions.Cookie.SecurePolicy = securePolicy;
                });

                options.TwoFactorRememberMeCookie?.Configure(cookieOptions =>
                {
                    cookieOptions.Cookie.HttpOnly = true;
                    cookieOptions.Cookie.SameSite = SameSiteMode.Strict;
                    cookieOptions.Cookie.SecurePolicy = securePolicy;
                });

                options.TwoFactorUserIdCookie?.Configure(cookieOptions =>
                {
                    cookieOptions.Cookie.HttpOnly = true;
                    cookieOptions.Cookie.SameSite = SameSiteMode.Strict;
                    cookieOptions.Cookie.SecurePolicy = securePolicy;
                });
            });
        
        services.AddIdentityCore<UserModel>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;

            // Password requirements - Production security standards
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 12; // Minimum 12 characters
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings - Prevent brute force attacks
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); // Lock for 15 minutes
            options.Lockout.MaxFailedAccessAttempts = 5; // Lock after 5 failed attempts
            options.Lockout.AllowedForNewUsers = true; // Enable lockout for new users

            // User settings
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        })
            .AddEntityFrameworkStores<ShireBudgetersDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<IIdentityService, IdentityService>()
                   .AddScoped<ICategoryService, CategoryService>()
                   .AddScoped<IPostService, PostService>()
                   .AddScoped<ILeadMagnetService, LeadMagnetService>();
}
