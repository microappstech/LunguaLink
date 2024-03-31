using Langua.Auth;
using Langua.DataContext.Data;
using Langua.ModelView.InputModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Langua.WebUI.Pages.Auth
{
    public partial class LoginComponent: BasePage
    {
        [Inject] SignInManager<ApplicationUser> SignInManager { get; set; }
        public string errorMessage { get; set; }
        public LoginInput Input { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Input = new LoginInput();
        }
        public async Task LoginUser()
        {
            var result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {

                Navigation.NavigateTo("/", true);
            }
            ////var result =await Security.Login(Input);
            //if (result.IsSucced())
            //{
            //    Navigation.NavigateTo("/", true);
            //}
            else
            {
                Notify("Login Failed", "result.Message", Radzen.NotificationSeverity.Error);
            }
        }

    }
}