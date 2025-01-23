using Langua.Models;
using Langua.WebUI.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;

namespace Langua.WebUI.Data
{
    public class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;
        private readonly PersistentComponentState _state;
        private Task<AuthenticationState> _authentificationTask;
        private readonly PersistingComponentStateSubscription stateSubscription;
        public readonly ProtectedSessionStorage sessionStorage;
        public PersistingRevalidatingAuthenticationStateProvider(
            ILoggerFactory loggerFactory, 
            ProtectedSessionStorage sessionStorage,
            IServiceScopeFactory scopeFactory, 
            PersistentComponentState persistentComponentState,
            IOptions<IdentityOptions> optionsAccessor
            )
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _state = persistentComponentState;
            _options = optionsAccessor.Value;

            this.sessionStorage = sessionStorage;
            AuthenticationStateChanged += OnAuthenticationStateChanged;
            stateSubscription = _state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }
        private void OnAuthenticationStateChanged( Task<AuthenticationState> task )
        {
            _authentificationTask = task;
        }

        private async Task OnPersistingAsync()
        {
            if(_authentificationTask is null)
            {
                throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
            }
            var authenticationState = await _authentificationTask;
            var principal = authenticationState.User;
            if (principal?.Identity?.IsAuthenticated == true)
            {
                var userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
                var email = principal.FindFirst(_options.ClaimsIdentity.EmailClaimType)?.Value;
                if(userId != null && email != null)
                {
                    _state.PersistAsJson(nameof(UserInfo),new UserInfo
                    {
                        Email = email,
                        UserId = userId
                    });
                }
            }
        }
        protected override TimeSpan RevalidationInterval => TimeSpan.FromHours(1);

        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> manager, ClaimsPrincipal principal)
        {
            var user = await manager.GetUserAsync(principal);
            if (user == null)
                return false;
            else if (!manager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await manager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }
        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }
        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    var sess = await sessionStorage.GetAsync<string>("Token");
        //    var Token = sess.Success ? sess.Value : null;
        //    if (Token == null)
        //        return null;
            
        //            notig
                


        //}
    }
}
