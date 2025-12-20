using Microsoft.EntityFrameworkCore;
using ShireBudgeters.Components;
using ShireBudgeters.Configurations;
using ShireBudgeters.DA.Configurations.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebAppServices(builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Apply any pending migrations at startup
using var scope = app.Services.CreateScope();
using var context = scope.ServiceProvider.GetRequiredService<ShireBudgetersDbContext>();
context.Database.Migrate();

app.Run();
