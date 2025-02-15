using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Manager;
using Langua.WebUI.Pages.Teachers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;

namespace Langua.WebUI.Pages.Managers
{
    public partial class ManagersComponent : BasePage
    {
        public IEnumerable<Models.Manager>? Managers { get; set; }
        public RadzenDataGrid<Models.Manager>? grid0;
        protected bool isLoading;
        [Inject] private IRepositoryCrudBase<Models.Manager>? baseRepository { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;
            var ManagersResult = baseRepository!.GetAll();
            if (ManagersResult.Succeeded)
            {
               var result = await baseService.Apply<Models.Manager>(ManagersResult.Value, new QueryCollection(new Dictionary<string, StringValues> { { "include", "Department" } }));
                Managers = (IEnumerable<Models.Manager>)result;
            }
            await InvokeAsync(StateHasChanged);
            }
            finally
            {
                isLoading = false;
            }
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
            var result = await dialogService.OpenAsync<CUManager>("Edit the manager", new Dictionary<string, object> { { "Id", mng.Id },{ "IsEdit",true } }, new DialogOptions { Width = "50vw", ShowClose = true });
            await grid0!.Reload();
        }
        public async Task Add()
        {
            var result = await dialogService.OpenAsync<CUManager>("Add new manager", new Dictionary<string, object> { { "IsEdit",false} }, new DialogOptions { Width = "50vw", ShowClose = true });
            await grid0!.Reload();
        }
    }
}
