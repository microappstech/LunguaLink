using Langua.WebUI.Client.LocalRessources;
using Langua.WebUI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Net.NetworkInformation;

using System.Text;

namespace Langua.WebUI.Client.Pages
{
    public class BasePageClient :ComponentBase
    {
        public string LanguaUrl 
        { 
            get
            {
                return navigationManager!.BaseUri.ToString();
            } 
        }
        [Inject] protected IJSRuntime JSRuntime { get; set; } = null!;

        [Inject] protected NavigationManager navigationManager { get; set; } = null!;

        [Inject] protected DialogService dialogService { get; set; } = null!;

        [Inject] protected TooltipService tooltipService { get; set; } = null!;

        [Inject] protected ContextMenuService? contextMenuService { get; set; }

        [Inject] protected NotificationService notificationService { get; set; } = null!;

        [Inject] public LangClientService? LangClientService { get; set; }
        [Inject] public Microsoft.Extensions.Localization.IStringLocalizer<LanguaResource> L { get; set; }

        public virtual void Notify(string title, string message, NotificationSeverity notification , double Time = 4000)
        {
            notificationService!.Notify(notification, title, message, Time);
        }
        public virtual void NotifyError(string title, string message, double Time = 4000)
        {
            notificationService!.Notify(NotificationSeverity.Error, title, message, Time);
        }
        public virtual void NotifySuccess(string title, string message, double Time = 4000)
        {
            notificationService!.Notify(NotificationSeverity.Success, title, message, Time);
        }
        public virtual async Task<bool?> Confirm(string title, string message, ConfirmOptions confirmOptions = default(ConfirmOptions)!)
        {
            if (confirmOptions is null)
            {
                confirmOptions = new ConfirmOptions { Width = "400px", OkButtonText = L["Confirm"], CancelButtonText = L["Cancel"] };
            }
            return await dialogService!.Confirm(message, title, confirmOptions);
        }
        public async Task LogMessage(string message)
        {
            await JSRuntime.InvokeVoidAsync("console.log", message);
        }
        public string GenerateVerificationCode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder code = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                code.Append(chars[random.Next(chars.Length)]);
            }
            return code.ToString();
        }
    }
}
