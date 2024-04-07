using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Langua.Account;

namespace Langua.WebUI.Pages.Layout
{
    public partial class SideMainLayout
    {
        [Inject] SecurityService security { get; set; }
        protected async Task Logout()
        {
            await security.Logut();
        }
    }
}