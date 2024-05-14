using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Manager;
using Langua.WebUI.Pages.Teachers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using Radzen;
using Radzen.Blazor;

namespace Langua.WebUI.Pages.Managers
{
    public partial class ManagersComponent : BasePage
    {
        public IEnumerable<Models.Manager>? Managers { get; set; }
        public RadzenDataGrid<Models.Manager>? grid0;
        [Inject] private IRepositoryCrudBase<Models.Manager>? baseRepository { get; set; }
        protected override Task OnInitializedAsync()
        {
            var ManagersResult = baseRepository!.GetAll();
            if (ManagersResult.Succeeded)
            {
                Managers = ManagersResult.Value;
            }
            return base.OnInitializedAsync();
        }
        public async Task Delete(Models.Manager mng)
        {
            if (await Confirm(L["Confirmation"], L["Are you sure want to delete this manager"]) == true)
            {
                var resultDelete = baseRepository!.Delete(mng);
                if (resultDelete.Succeeded)
                {
                    Notify("Success", "Suppression successfully finished", NotificationSeverity.Success);
                    dialogService.Close();
                    await grid0!.Reload();
                }

            }
        }
        public async Task Edit(Models.Manager mng)
        {
            var result = await dialogService.OpenAsync<CUManager>("Edit the manager", new Dictionary<string, object> { { "Id", mng.Id } }, new DialogOptions { Width = "50vw", ShowClose = true });
            await grid0!.Reload();
        }
        public async Task Add()
        {
            var result = await dialogService.OpenAsync<CUManager>("Add new manager", null, new DialogOptions { Width = "50vw", ShowClose = true });
            await grid0!.Reload();
        }
    }
}
