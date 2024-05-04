using Langua.Models;
using Langua.Shared.Constants;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Langua.WebUI.Client.Pages.Sessions
{
    public partial class SchedulerComponent : BasePageClient
    {
        protected RadzenScheduler<Session>? scheduler;
        
        public IEnumerable<Session>? Sessions { get; set; }
        public ICollection<Teacher>? Teachers { get; set; }
        public ICollection<Groups>? Groups { get; set; }
        public int selectedTeacher { get; set; }
        public int selectedGroup { get; set; }
        public Groups? _Group { get; set; }
        public bool SchedelurVisible { get; set; }
        public string? Title
        {
            get {
                if (SchedelurFor.TeacherSched == ScheFor)
                    return L["Teacher Schedelur"];
                return L["Group Schedelur"];
                }
            set {}
        }
        public async Task SelectChanged()
        {
            switch (ScheFor)
            {
                case SchedelurFor.TeacherSched:
                    var sched = await LangClientService!.GetSessions(filter: $"TeacherId eq '{selectedTeacher}'");
                    Sessions = sched;
                    break;
                case SchedelurFor.GroupSched:
                    var sessions = await LangClientService!.GetSessions(filter: $"GroupId eq '{selectedGroup}'");
                    Sessions = sessions;
                    break;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            var teachers =await LangClientService.GetTeachers();
            var groups =await LangClientService.GetGroups();
            Teachers = teachers.ToList();
            Groups = groups.ToList();

            
        }
        public SchedelurFor ScheFor { get; set; }
        public void ShedelurChanged()
        {
            if(ScheFor == SchedelurFor.TeacherSched)
            {
                Title = L["Teacher Schedelur"];
            }
            else
            {
                Title = L["Group Schedelur"];
            }
        }
        protected void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            
        }

        protected async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            
        }

        protected async Task OnSessionSelect(SchedulerAppointmentSelectEventArgs<Session> args)
        {
            
        }

        protected void OnSessionRender(SchedulerAppointmentRenderEventArgs<Session> args)
        {
            
        }

        protected async Task OnSessionMove(SchedulerAppointmentMoveEventArgs args)
        {

        }
    }
}