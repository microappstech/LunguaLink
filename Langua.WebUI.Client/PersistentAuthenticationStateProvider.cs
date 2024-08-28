using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Langua.WebUI.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> defaultUnauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        private readonly Task<AuthenticationState> authenticationStateTask = defaultUnauthenticatedTask;

        public CustomAuthenticationStateProvider(PersistentComponentState state)
        {
            if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
            {
                return;
            }

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email) ];

            authenticationStateTask = Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    authenticationType: nameof(CustomAuthenticationStateProvider)))));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => authenticationStateTask;
    }




    //public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider
    //{

    //    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        var identity = new ClaimsIdentity();

    //        try
    //        {
    //            var state = await GetApplicationAuthenticationStateAsync();

    //            if (state.IsAuthenticated)
    //            {
    //                identity = new ClaimsIdentity(state.Claims.Select(c => new Claim(c.Type, c.Value)), "FourProjects.Server");
    //            }
    //        }
    //        catch (HttpRequestException ex)
    //        {
    //        }

    //        var result = new AuthenticationState(new ClaimsPrincipal(identity));

    //        await securityService.InitializeAsync(result);

    //        return result;
    //    }

    //    private async Task<ApplicationAuthenticationState> GetApplicationAuthenticationStateAsync()
    //    {
    //        if (authenticationState == null)
    //        {
    //            authenticationState = await securityService.GetAuthenticationStateAsync();
    //        }

    //        return authenticationState;
    //    }
    //}
}
