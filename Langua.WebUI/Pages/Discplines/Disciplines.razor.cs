
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;
using Radzen;
using Radzen.Blazor;

namespace Langua.WebUI.Pages.Discplines
{
    public partial class DisciplinesComponent:BasePage
    {
        [Inject] protected IRepositoryCrudBase<Subject> baseRepository { get; set; }
        public RadzenDataGrid<Subject> grid0;
        public bool DataReady { get; set; } = false;
        protected IEnumerable<Subject> subjects { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {

                await Security.InitializeAsync();
                var subjectsResult = LanguaService.GetSubjects();
                if(subjectsResult.Succeeded)
                {
                    subjects = subjectsResult.Value;
                }
            }
            finally
            {
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
                    await grid0.Reload();
                    dialogService.Close(subject);
                }
            }
        }
        public async Task Edit(Subject _subject)
        {
            var result = await dialogService.OpenAsync<EditDiscipline>("Edit the subject", new Dictionary<string, object> { { "Id", _subject.Id } });
            await grid0.Reload();

        }
        public async Task SubmitAdd()
        {
            var result = await dialogService.OpenAsync<AddDiscpline>("Create New Subject", null);

            await grid0.Reload();
        }

    }
}