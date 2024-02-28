using Microsoft.AspNetCore.Components;
using Radzen;

namespace Langua.WebUI.Pages
{
    public partial class BasePage : ComponentBase
    {
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] DialogService DialogService { get; set; }
        [Inject] NotificationService NotificationService { get; set; }

        public virtual async Task<bool?> Confirm(string title, string message , ConfirmOptions confirmOptions)
        {
            if(confirmOptions is null)
            {
                confirmOptions = new ConfirmOptions { Width = "400px" };
            }
           return await DialogService.Confirm(message,title,confirmOptions);
        }
        public virtual void Notify(string title, string message, NotificationSeverity Severity = NotificationSeverity.Info)
        {
            NotificationService.Notify(Severity,message,title,4000);
        }
        
    }
}
