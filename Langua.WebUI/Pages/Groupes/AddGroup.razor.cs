
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
namespace Langua.WebUI.Pages.Groupes
{
    public partial class AddGroupComponent:BasePage
    {
        public Groups group { get; set; }
        [Inject] public IRepositoryCrudBase<Groups> repository { get; set; }
        public IEnumerable<Department> Departments { get; set; }    


        protected override async Task OnInitializedAsync()
        {
            group = new Groups();
            var ResultDep = await LanguaService.GetDepartement();
            if(ResultDep.Succeeded) 
                this.Departments = ResultDep.Value;
        }
        public async Task Submit(Groups gr)
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