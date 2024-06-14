using Langua.WebUI.Client.LocalRessources;
using Langua.WebUI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

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
        [Inject] protected IJSRuntime? JSRuntime { get; set; }

        [Inject] protected NavigationManager? navigationManager { get; set; }

        [Inject] protected DialogService dialogService { get; set; } = null!;

        [Inject] protected TooltipService tooltipService { get; set; } = null!;

        [Inject] protected ContextMenuService? contextMenuService { get; set; }

        [Inject] protected NotificationService notificationService { get; set; } = null!;

        [Inject] public LangClientService? LangClientService { get; set; }
        [Inject] public Microsoft.Extensions.Localization.IStringLocalizer<LanguaResource> L { get; set; }

        public virtual void Notify(string title, string message,NotificationSeverity notification = NotificationSeverity.Success, double Time=4000)
        {
            notificationService!.Notify(notification, title, message, Time);
        }
        public virtual async Task<bool?> Confirm(string title, string message, ConfirmOptions confirmOptions = default(ConfirmOptions)!)
        {
            if (confirmOptions is null)
            {
                confirmOptions = new ConfirmOptions { Width = "400px", OkButtonText = L["Confirm"], CancelButtonText = L["Cancel"] };
            }
            return await dialogService!.Confirm(message, title, confirmOptions);
        }
    }
}
