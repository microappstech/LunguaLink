using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Pages.Account.Shared
{
    public partial class ExternalLoginPicker
    {
        private AuthenticationScheme[] externalLogins = [];

        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            externalLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync()).ToArray();
        }
    }
}