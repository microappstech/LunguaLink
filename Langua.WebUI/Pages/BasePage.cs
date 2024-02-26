using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Pages
{
    public partial class BasePage : ComponentBase
    {
        [Inject] NavigationManager Navigation { get; set; }

    }
}
