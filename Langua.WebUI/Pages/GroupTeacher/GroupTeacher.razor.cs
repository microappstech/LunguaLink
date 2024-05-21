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
            var result = repository!.GetAll();
            if (result.Succeeded)
            {

                var grcand = await baseService!.Apply(result.Value, new QueryCollection(new Dictionary<string, StringValues> { { "include", "Group,Teacher" } }));
                GroupTeachers = grcand.Cast<Models.GroupTeacher>().ToList();
                DataReady = true;
            }
        }


        public async Task SubmitAdd()
        {
            var result = await dialogService.OpenAsync<CUGroupTeacher>("Create Group/Teacher",new Dictionary<string, object> { { "EntId",""} });
            await grid!.Reload();
        }
        public async Task Edit(Models.GroupTeacher groupTeacher)
        {
            var result = await dialogService.OpenAsync<CUGroupTeacher>("Edit Group/Teacher",new Dictionary<string, object> { { "EntId",groupTeacher.Id } });
            await grid!.Reload();
        }

        public async Task Delete(Models.GroupTeacher groupTeacher)
        {
            if(await Confirm(L["Delete Confirmation"], L["Are you sure want to delete this item"]) == true)
            {
                var result = repository!.Delete(groupTeacher);
                if (result.Succeeded)
                {
                    Notify(L["Success"], L["Deletion successfully completed"], NotificationSeverity.Success);
                    await grid!.Reload();
                }
                else
                {
                    Notify(L["Failed"], L["Deletion failed"], NotificationSeverity.Error);
                }
            }
            dialogService.Close();
        }
    }
}