using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ShireBudgeters.BL.Services.Identity;

public static class IdentityComponentsEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");

        // Logout Endpoint
        accountGroup.MapGet("/Logout", async (
            HttpContext context,
            [FromServices] IIdentityService identityService,
            string? returnUrl) =>
        {
            await identityService.LogoutAsync();
            return TypedResults.LocalRedirect($"~/{returnUrl ?? ""}");
        }).RequireAuthorization();

        return accountGroup;
    }
}
