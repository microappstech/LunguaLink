using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using Radzen.Blazor;

namespace Langua.WebUI.Pages.GroupTeacher
{
    public partial class GroupTeacherComponent:BasePage
    {
        [Inject] public IRepositoryCrudBase<Models.GroupTeacher> repository {  get; set; }
        [Inject] private BaseService baseService { get; set; }
        public bool DataReady { get; set; }
        public RadzenDataGrid<Models.GroupTeacher> grid;
        public IEnumerable<Models.GroupTeacher> GroupTeachers { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = repository.GetAll();
            if (result.Succeeded)
            {
                GroupTeachers = result.Value;
                baseService.Apply(result.Value, new QueryCollection(new Dictionary<string, StringValues> { { "include", "Group,Teacher" } }));
                DataReady = true;
            }
        }


        public async Task SubmitAdd()
        {
            var result = await dialogService.OpenAsync<CUGroupTeacher>("Create Group/Teacher",new Dictionary<string, object> { { "EntId",""} });
        }
    }
}