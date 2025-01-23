using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Teachers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using Radzen;
using Radzen.Blazor;

namespace Langua.WebUI.Pages.Teachers
{
    public partial class TeachersComponent : BasePage
    {
        public IEnumerable<Teacher>? teachers { get; set; }
        public RadzenDataGrid<Teacher>? grid0;
        public bool _AddBtnClicked;
        
        [Inject] private IRepositoryCrudBase<Teacher>? baseRepository { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadTeachers();
        }
        public async Task LoadTeachers()
        {
            var TeachersResult = await LanguaService.GetTeachers(includes: "Departement");
            if (TeachersResult.Succeeded)
            {
                teachers = TeachersResult.Value;
            }
        }
        public async Task Delete(Teacher teacher)
        {
            if (await Confirm(L["Confirmation"], L["Are you sure want to delete this teacher"]) == true)
            {
                var resultDelete = baseRepository!.Delete(teacher);
                if (resultDelete.Succeeded)
                {
                    Notify("Success", "Suppression successfully finished", NotificationSeverity.Success);
                    await LoadTeachers();
                }

            }
        }
        public async Task ResetPassword(Teacher teacher)
        {
            var res = await Security.ResetPassword(teacher!.Email);
            if (res.Item1)
                NotifySuccess(L["Success"], res.Item2);
            else
                NotifyError(L["Error"], L["Something working, Try again"]);
        }
        public async Task Edit(Teacher teacher)
        {

            Teacher t = null;
            _ = t.Departement;
            var result = await dialogService.OpenAsync<Langua.WebUI.Pages.Teachers.EditTeacher>("Edit the teacher", new Dictionary<string, object> { { "Id", teacher.Id } }, new DialogOptions { Width = "50vw", ShowClose = true });
            await LoadTeachers();
            _AddBtnClicked = false;
        }
        public async Task Add()
        {
            try
            {
                Teacher t = null;
                _ = t.Departement;
                _AddBtnClicked = true;
                var result = await dialogService.OpenAsync<AddTeacher>("Add new teacher", null, new DialogOptions { Width = "50vw", ShowClose = true });
                await LoadTeachers();
                _AddBtnClicked = false;

            }catch(Exception ex)
            {

            }
        }
    }
}
