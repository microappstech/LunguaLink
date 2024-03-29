using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Pages.Account.Shared
{
    public partial class StatusMessage
    {
        private string? messageFromCookie;

        [Parameter]
        public string? Message { get; set; }

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        private string? DisplayMessage => Message ?? messageFromCookie;

        protected override void OnInitialized()
        {
            messageFromCookie = HttpContext.Request.Cookies[IdentityRedirectManager.StatusCookieName];

            if (messageFromCookie is not null)
            {
                HttpContext.Response.Cookies.Delete(IdentityRedirectManager.StatusCookieName);
            }
        }
    }
}