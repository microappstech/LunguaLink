using Langua.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace Langua.WebUI.Data
{
    public static class SignoutEndPointBuilder
    {
        public static IEndpointConventionBuilder MapingOutEndpoint(this IEndpointRouteBuilder endpoint)
        {
            var accountGr = endpoint.MapGroup("/Account");
            accountGr.MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.Redirect("/Login");
            });
            return accountGr;

       }
    }
}
