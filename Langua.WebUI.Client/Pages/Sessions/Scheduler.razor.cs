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
        public bool SchedelurVisible { get; set; } = false;
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
            SchedelurVisible = false;
            if(selectedGroup !=0 || selectedTeacher !=0)
                SchedelurVisible = true;
            
            switch (ScheFor)
            {
                case SchedelurFor.TeacherSched:
                    var sched = await LangClientService!.GetSessions(); //(filter: $"TeacherId eq '{selectedTeacher}'");
                    Sessions = sched.Where(i=>i.TeacherId == selectedTeacher);
                    break;
                case SchedelurFor.GroupSched:
                    var sessions = await LangClientService!.GetSessions(); //(filter: $"GroupId eq '{selectedGroup}'");
                    Sessions = sessions.Where(s=> s.GroupId == selectedGroup);
                    break;
            }
            await scheduler.Reload();
        }
        protected override async Task OnInitializedAsync()
        {
            var teachers =await LangClientService.GetTeachers();
            var groups =await LangClientService.GetGroups();
            Teachers = teachers.ToList();
            Groups = groups.ToList();
            if (Groups.FirstOrDefault() is not null)
                selectedGroup = Groups.FirstOrDefault().Id;
            if(Teachers.FirstOrDefault() is not null)
                selectedTeacher = teachers.FirstOrDefault().Id;
            var sessions = await LangClientService!.GetSessions();
            Sessions = sessions;


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
            var data = await dialogService.OpenAsync<FormSession>(L["Add session"], new Dictionary<string, object>
            {
                { "IsForGroup", ScheFor == SchedelurFor.GroupSched },
                { "GroupOrTeacherId", (ScheFor == SchedelurFor.GroupSched ? selectedGroup:selectedTeacher) },
                { "End" , args.End },
                { "Start" , args.Start }
            }) ;
            await scheduler.Reload();
        }

        protected async Task OnSessionSelect(SchedulerAppointmentSelectEventArgs<Session> args)
        {
            var data = await dialogService.OpenAsync<FormSession>(L["EditSession session"], new Dictionary<string, object>{{"SessionId",args.Data.Id }, { "IsForGroup", ScheFor == SchedelurFor.GroupSched }, { "GroupOrTeacherId", (ScheFor == SchedelurFor.GroupSched ? selectedGroup : selectedTeacher) } });
            await scheduler.Reload();
        }

        protected void OnSessionRender(SchedulerAppointmentRenderEventArgs<Session> args)
        {
            
        }

        protected async Task OnSessionMove(SchedulerAppointmentMoveEventArgs args)
        {

        }
    }
}