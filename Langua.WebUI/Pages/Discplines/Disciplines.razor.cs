
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Radzen;

namespace Langua.WebUI.Pages.Discplines
{
    public partial class DisciplinesComponent:BasePage
    {
        [Inject] protected IRepositoryCrudBase<Subject> baseRepository { get; set; }
        public bool DataReady { get; set; } = false;
        protected IEnumerable<Subject> subjects { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var subjectsResult = baseRepository.GetAll();
            if(subjectsResult.Succeeded)
            {
                subjects = subjectsResult.Value;
                DataReady = true;
            }
            
            
            
            
        }

        public async Task Delete(Subject subject)
        {
            if (await Confirm("Confirmation Deletion","Are sure you want to delete this Subject") == true)
            {
                var result = baseRepository.Delete(subject);
                if (result.Succeeded)
                {
                    Notify("Successful Deleted", "The subject Successfully deleted", NotificationSeverity.Success);
                    dialogService.Close(subject);
                }
            }
        }
        public async Task Edit(Subject _subject)
        {

        }
        public async Task SubmitAdd()
        {
            var result = await dialogService.OpenAsync<Langua.WebUI.Pages.Discplines.AddDiscpline>("Create New Subject", null);
            await InvokeAsync(StateHasChanged);
        }

    }
}