
using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Pages.GroupTeacher
{
    public partial class CUGroupTeacherComponent:BasePage
    {
        [Parameter] public dynamic? EntId { get; set; }
        [Inject] BaseService baseService { get; set; }
        [Inject] IRepositoryCrudBase<Models.GroupTeacher> repository { get; set; }
        [Inject] IRepositoryCrudBase<Groups> groupRepository { get; set; }
        [Inject] IRepositoryCrudBase<Teacher> teacherRepository { get; set; }
        public Models.GroupTeacher groupTeacher { get; set; }
        public IEnumerable<Groups> Groups { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public bool DataReady = false;

        protected override async Task OnInitializedAsync()
        {
            var GResult = groupRepository.GetAll();
            var Treasult = teacherRepository.GetAll();
            if(GResult.Succeeded && Treasult.Succeeded)
            {
                Groups = GResult.Value;
                Teachers = Treasult.Value;
            }
            bool converted = int.TryParse(EntId, out int GroupTeacherId);
            if (!converted)
            {
                groupTeacher = new Models.GroupTeacher();
            }
            else
            {
                var result = repository.GetById(GroupTeacherId) ;
                if(result.Succeeded)
                {
                    groupTeacher = result.Value;
                    DataReady = true;
                }
            }
        }
        public async Task HandleValidSubmit()
        {
            if (groupTeacher.Id == 0)
            {
                var result = repository.Add(groupTeacher);
                if (result.Succeeded)
                {
                    Notify("Success", "Item created successfully", Radzen.NotificationSeverity.Success);
                    dialogService.Close();
                }
            }
            else
            {
                var result = repository.Update(groupTeacher);
                if (result.Succeeded)
                {
                    Notify("Success", "Item updated successfully", Radzen.NotificationSeverity.Success);
                    dialogService.Close();
                }
            }
        }
    }
}