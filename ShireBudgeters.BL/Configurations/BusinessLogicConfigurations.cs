using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShireBudgeters.Common.Common.Constants;
using ShireBudgeters.DA.Configurations;
using ShireBudgeters.DA.Configurations.Database;
using ShireBudgeters.DA.Models;
using System;

namespace ShireBudgeters.BL.Configurations;

public static class BusinessLogicConfigurations
{
    public static IServiceCollection AddBusinessLogicServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isProduction)
    {
        services.AddDataAccessServices(configuration);
        services.AddAuthNAndAuthZ(isProduction);
        return services;
    }

    private static IServiceCollection AddAuthNAndAuthZ(this IServiceCollection services, bool isProduction)
    {
        services.AddAuthentication().AddCookie(options =>
        {
            options.Cookie.Name = Identity.CookieName;
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);

            options.LoginPath = Identity.LoginPath;
            options.AccessDeniedPath = Identity.AccessDeniedPath;
            options.SlidingExpiration = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = isProduction
                ? CookieSecurePolicy.Always
                : CookieSecurePolicy.SameAsRequest;
        });

        services.AddAuthorization();

        services.AddIdentityCore<UserModel>(options => { })
            .AddUserManager<UserManager<UserModel>>()
            .AddSignInManager<SignInManager<UserModel>>()
            .AddEntityFrameworkStores<ShireBudgetersDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 12;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        });

        return services;
    }
}
