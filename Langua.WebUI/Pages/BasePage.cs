using Langua.Models;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Langua.Repositories.Interfaces;
using Radzen;
using Langua.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Langua.WebUI.Client.Pages;
using Radzen.Blazor;

namespace Langua.WebUI.Pages
{
    public partial class BasePage : BasePageClient
    {
        [Inject] public NavigationManager Navigation { get; set; } = null!;
        public RadzenProgressBarCircular? loading;
        public bool IsStuck { get; set; } = true;
        [Inject] protected Langua.Account.SecurityService Security { get; set; } = null!;
        [Inject] public LanguaService LanguaService { get; set; } = null!;
        [Inject] public BaseService baseService { get; set; } = null!;
        [Inject] protected AuthenticationStateProvider? authenticationStateProvider { get; set; }
        [Inject] protected IMailService? mailService { get; set; }
        
    }
}
