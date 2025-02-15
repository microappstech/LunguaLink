using Langua.Account;
using Langua.DataContext.Data;
using Langua.ModelView.InputModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Langua.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Langua.WebUI.Client.Localization;

namespace Langua.WebUI.Pages.Auth
{
    public partial class LoginComponent: BasePage
    {

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;
        
        public string? errorMessage { get; set; }
        [SupplyParameterFromForm]
        public LoginInput Input { get; set; } = new();
        [Inject] public Microsoft.Extensions.Localization.IStringLocalizer<Localizer> L { get; set; }

        public EditContext? EditLogin;
        protected override async Task OnInitializedAsync()
        {
            if (this.HttpContext is not null)
            {
                if (HttpMethods.IsGet(HttpContext.Request.Method))
                {
                    // Clear the existing external cookie to ensure a clean login process
                }
            }
            Input = new LoginInput()
            {
                Email = "Hamzamouddakur@gmail.com",
                Password = "Hamza=Langua123"
            };
            await LoginUser();
        }
        public async Task LoginUser()
        {
            
            var result = await Security.Login(Input);
            if (result.IsSucced())
            {
                Navigation.NavigateTo(result.RedirectUrl, true);
            }
            else
            {
                errorMessage = L["TxtWrongUserOrPassword"];
            }
        }

    }
}