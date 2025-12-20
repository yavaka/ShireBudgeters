using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ShireBudgeters.DA.Models;
using System.Security.Claims;

namespace ShireBudgeters.BL.Services.Identity;

public static class IdentityComponentsEndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");

        accountGroup.MapPost("/Logout", async (
            HttpContext context,
            [FromServices] IIdentityService identityService,
            [FromServices] IAntiforgery antiforgery,
            [FromForm] string? returnUrl) =>
        {
            await antiforgery.ValidateRequestAsync(context);
            await identityService.LogoutAsync();
            return TypedResults.LocalRedirect($"~/{returnUrl ?? ""}");
        });

        return accountGroup;
    }
}
