using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Pages.Account.Pages.Manage
{
    public partial class PersonalData
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            _ = await UserAccessor.GetRequiredUserAsync(HttpContext);
        }
    }
}