using Microsoft.EntityFrameworkCore;
using Serilog;
using ShireBudgeters.BL.Services.Identity;
using ShireBudgeters.Common.Middleware;
using ShireBudgeters.Components;
using ShireBudgeters.Configurations;
using ShireBudgeters.DA.Configurations.Database;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddWebAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // HSTS: Force HTTPS for 1 year in production
    app.UseHsts();
}

// Add security headers middleware early in the pipeline
app.UseMiddleware<SecurityHeadersMiddleware>();

// Add IP address logging middleware early in the pipeline
app.UseMiddleware<IpAddressLoggingMiddleware>();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// HTTPS Redirection - redirects HTTP to HTTPS
// In production, this enforces HTTPS for all requests
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Apply any pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ShireBudgetersDbContext>();
    context.Database.Migrate();

    // Seed the database if configured
    var seedDatabase = app.Configuration.GetValue<bool>("DatabaseSeeder:Enabled", defaultValue: false);
    var seedSampleData = app.Configuration.GetValue<bool>("DatabaseSeeder:SeedSampleData", defaultValue: false);

    if (seedDatabase)
    {
        var seeder = scope.ServiceProvider.GetRequiredService<ShireBudgeters.DA.Configurations.Database.DatabaseSeeder>();
        await seeder.SeedAsync(seedSampleData);
    }
}

app.MapAdditionalIdentityEndpoints();

app.Run();
