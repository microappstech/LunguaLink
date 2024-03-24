using Langua.Models;
using Microsoft.AspNetCore.Components;
using Langua.Repositories.Interfaces;
using System.Linq.Dynamic.Core;

namespace Langua.WebUI.Pages.Groupes
{
    public partial class EditGroupComponent:BasePage
    {
        [Parameter]
        public int Id { get; set; }
        public Groups group { get; set; }
        public bool DataReady { get; set; }
        [Inject] public IRepositoryCrudBase<Groups> repository { get; set; }


        protected override async Task OnInitializedAsync()
        {
            var GroupRsult = repository.GetById(Convert.ToInt32(Id));
            if (GroupRsult.Succeeded)
            {
                group = GroupRsult.Value;
                DataReady = true;
            }
        }
        public async Task Submit()
        {
            var GResult = repository.Update(group);
            if (GResult.Succeeded)
            {
                Notify("Success", "Group Successfuly updated", Radzen.NotificationSeverity.Success);
                dialogService.Close();
            }

        }
    }
}