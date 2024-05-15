using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Discplines;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System;

namespace Langua.WebUI.Pages.Departements
{
    public partial class DepartementsComponent:BasePage
    {
        [Inject]
        public IRepositoryCrudBase<Department> baseRepository { get; set; }
        protected RadzenDataGrid<Department>? grid0;
        public IEnumerable<Department>? Departements { get; set; }


        protected override Task OnInitializedAsync()
        {
            var result = baseRepository.GetAll();
            Departements = result.Value;
            return base.OnInitializedAsync();
        }
        public async Task SubmitAdd()
        {
            var result = await dialogService.OpenAsync<CUDepartement>("Create New Departement", new Dictionary<string, object> { { "isEdit",false} });

            await grid0!.Reload();
        }

        public async Task Edit(Department dep)
        {
            var result = await dialogService.OpenAsync<CUDepartement>("Edit the departement", new Dictionary<string, object> { { "Id", dep.Id }, { "isEdit", true } });
            await grid0!.Reload();

        }

        public async Task Delete(Department dep)
        {
            if (await Confirm("Confirmation Deletion", "Are sure you want to delete this departement") == true)
            {
                var result = baseRepository.Delete(dep);
                if (result.Succeeded)
                {
                    Notify("Successful Deleted", "The depertement successfully deleted", NotificationSeverity.Success);
                    await grid0!.Reload();
                    dialogService.Close(dep);
                }
            }
        }
    }
}