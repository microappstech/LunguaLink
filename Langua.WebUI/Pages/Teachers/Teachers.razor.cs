using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Teachers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Langua.WebUI.Pages.Teachers
{
    public partial class TeachersComponent : BasePage
    {
        public IEnumerable<Teacher> teachers { get; set; }
        [Inject] private IRepositoryCrudBase<Teacher> baseRepository { get; set; }
        protected override Task OnInitializedAsync()
        {
            var TeachersResult = baseRepository.GetAll();
            teachers = TeachersResult.Value;
            return base.OnInitializedAsync();
        }
        public async Task Delete(Teacher teacher)
        {
            if (await Confirm(L["Confirmation"], L["Are you sure want to delete this teacher"]) == true)
            {
                var resultDelete = baseRepository.Delete(teacher);
                if (resultDelete.Succeeded)
                {
                    Notify("Success", "Suppression successfully finished", NotificationSeverity.Success);
                    dialogService.Close();
                }

            }
        }
        public async Task Edit(Teacher teacher)
        {
            var result = await dialogService.OpenAsync<EditTeacher>("Edit the teacher", new Dictionary<string, object> { { "Id", teacher.Id } }, new DialogOptions { Width = "50vw", ShowClose = true });
        }
        public async Task Add()
        {
            var result = await dialogService.OpenAsync<AddTeacher>("Add new teacher", null, new DialogOptions { Width = "50vw", ShowClose = true });
            StateHasChanged();
        }
    }
}
