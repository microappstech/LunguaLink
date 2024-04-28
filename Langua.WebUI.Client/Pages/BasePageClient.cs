using Langua.WebUI.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace Langua.WebUI.Client.Pages
{
    public class BasePageClient :ComponentBase
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        [Inject] protected NavigationManager? navigationManager { get; set; }

        [Inject] protected DialogService? dialogService { get; set; }

        [Inject] protected TooltipService? tooltipService { get; set; }

        [Inject] protected ContextMenuService? contextMenuService { get; set; }

        [Inject] protected NotificationService? notificationService { get; set; }

        [Inject] public LangClientService? LangClientService { get; set; }

        public virtual void Notify(string title, string message,NotificationSeverity notification = NotificationSeverity.Success, double Time=4000)
        {
            notificationService.Notify(notification, title, message, Time);
        }
        
    }
}
