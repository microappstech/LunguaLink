using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Pages.Departements
{
    public partial class CUDepartementComponent:BasePage
    {

        [Parameter] public bool IsEdit { get; set; }
        [Inject] IRepositoryCrudBase<Models.Department>? crudRepository { get; set; }
        public Models.Department? Department { get; set; }
        [Parameter] public int? Id { get; set; }
        public List<string> Errors { get; set; } = new();
        protected override Task OnInitializedAsync()
        {
            if (!IsEdit || Id is null)
            {
                Department= new Models.Department();
            }
            else
            {
                var result = crudRepository!.GetById((int)(Id!));
                Department = result.Value;
            }
            return base.OnInitializedAsync();
        }
        public async Task HandleValidSubmit()
        {
            Errors = new();
            if (!IsEdit && Department!.Id == 0)
            {
                var result = crudRepository!.Add(Department);
                if (result.Succeeded)
                {
                    Notify("Success", "Item created successfully", Radzen.NotificationSeverity.Success);
                    dialogService.Close();
                }
                else
                {
                    Errors.Add(result.Error);
                }
            }
            else
            {
                var result = crudRepository!.Update(Department);
                if (result.Succeeded)
                {
                    Notify("Success", "Item updated successfully", Radzen.NotificationSeverity.Success);
                    dialogService.Close();
                }
                else
                {
                    Errors.Add(result.Error);
                }
            }
        }

    }
}