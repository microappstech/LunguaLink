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
        [Inject]public NavigationManager? Navigation { get; set; }
        public RadzenProgressBarCircular loading;
        public bool IsStuck { get; set; } = true;
        [Inject] protected SecurityService? Security { get; set; }
        [Inject] public LanguaService LanguaService { get; set; }
        [Inject] public BaseService? baseService { get; set; }
        [Inject] protected AuthenticationStateProvider? authenticationStateProvider { get; set; }
        [Inject] protected IMailService mailService { get; set; }
        public virtual async Task<bool?> Confirm(string title, string message , ConfirmOptions confirmOptions = default(ConfirmOptions))
        {
            if(confirmOptions is null)
            {
                confirmOptions = new ConfirmOptions { Width = "400px" , OkButtonText = L["Confirm"], CancelButtonText = L["Cancel"] };
            }
           return await dialogService.Confirm(message,title,confirmOptions);
        }
        public virtual void Notify(string title, string message, NotificationSeverity Severity = NotificationSeverity.Info)
        {
            notificationService.Notify(Severity,message,title,4000);
        }
        
    }
}
