
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
namespace Langua.WebUI.Pages.Groupes
{
    public partial class AddGroupComponent:BasePage
    {
        public Groups group { get; set; }
        [Inject] public IRepositoryCrudBase<Groups> repository { get; set; }


        protected override Task OnInitializedAsync()
        {
            group = new Groups();
            return base.OnInitializedAsync();
        }
        public async Task Submit()
        {
            var GResult = repository.Add(group);
            if(GResult.Succeeded)
            {
                Notify("Success", "Group Successfuly created", Radzen.NotificationSeverity.Success);
                dialogService.Close();
            }
        }
    }
}