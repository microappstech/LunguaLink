using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using Radzen;
using Radzen.Blazor;

namespace Langua.WebUI.Pages.GroupTeacher
{
    public partial class GroupTeacherComponent:BasePage
    {
        [Inject] public IRepositoryCrudBase<Models.GroupTeacher>? repository {  get; set; }
        public bool DataReady { get; set; }
        public RadzenDataGrid<Models.GroupTeacher>? grid;
        public IEnumerable<Models.GroupTeacher>? GroupTeachers { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;
                await LoadGrTeachers();

            }
            finally
            {
                isLoading = false;
            }
        }
        protected bool isLoading;
        public async Task LoadGrTeachers()
        {
            var result = await LanguaService.GetGroupTeachers(includes: "Group,Teacher");
            if (result.Succeeded)
            {
                GroupTeachers = result.Value.ToList();
                DataReady = true;
            }
        }


        public async Task SubmitAdd()
        {
            var result = await dialogService.OpenAsync<CUGroupTeacher>("Create Group/Teacher",new Dictionary<string, object> { { "EntId",""} });
            await LoadGrTeachers();
            await grid!.Reload();
        }
        public async Task Edit(Models.GroupTeacher groupTeacher)
        {
            var result = await dialogService.OpenAsync<CUGroupTeacher>("Edit Group/Teacher",new Dictionary<string, object> { { "EntId",groupTeacher.Id } });
            await LoadGrTeachers();
            await grid!.Reload();
        }

        public async Task Delete(Models.GroupTeacher groupTeacher)
        {
            if(await Confirm(L["Delete Confirmation"], L["Are you sure want to delete this item"]) == true)
            {
                var result = repository!.Delete(groupTeacher);
                if (result.Succeeded)
                {
                    await LoadGrTeachers();
                    Notify(L["Success"], L["Deletion successfully completed"], NotificationSeverity.Success);
                    await grid!.Reload();
                }
                else
                {
                    Notify(L["Failed"], L["Deletion failed"], NotificationSeverity.Error);
                }
            }
        }
    }
}