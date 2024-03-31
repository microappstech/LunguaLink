using Microsoft.AspNetCore.Identity;

namespace Langua.WebUI.Pages.Roles
{
    public partial class RolesComponenet:BasePage
    {

        public IEnumerable<IdentityRole> Roles { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var RoleResult = await Security.GetRoles();
            Roles = RoleResult.ToList();
        }

    }
}