using Langua.Auth;
using Langua.DataContext.Data;
using Langua.ModelView.InputModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Langua.WebUI.Pages.Auth
{
    public partial class LoginComponent: BasePage
    {

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;
        [Inject] SignInManager<ApplicationUser> SignInManager { get; set; }
        public string errorMessage { get; set; }
        [SupplyParameterFromForm]
        public LoginInput Input { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            if (this.HttpContext is not null)
            {
                if (HttpMethods.IsGet(HttpContext.Request.Method))
                {
                    // Clear the existing external cookie to ensure a clean login process
                    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                }
            }
        }
        public async Task LoginUser()
        {
            //var result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            //if (result.Succeeded)
            //{

            //    Navigation.NavigateTo("/", true);
            //}
            var result = await Security.Login(Input);
            if (result.IsSucced())
            {
                Navigation.NavigateTo("/", true);
            }
            else
            {
                Notify("Login Failed", "result.Message", Radzen.NotificationSeverity.Error);
            }
        }

    }
}