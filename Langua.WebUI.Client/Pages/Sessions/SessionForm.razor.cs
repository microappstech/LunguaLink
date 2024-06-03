using Langua.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace Langua.WebUI.Client.Pages.Sessions
{
    public partial class FormSession:BasePageClient
    {
        [Parameter] public int SessionId { get; set; }
        [Parameter] public int GroupOrTeacherId { get; set; }
        [Parameter] public bool IsForGroup { get; set; }
        [Parameter] public DateTime End { get; set; }
        [Parameter] public DateTime Start { get; set; }
        
        protected bool errorVisible;
        protected Models.Session? Session;

        protected IEnumerable<Models.Groups>? groupsForGroupId;

        protected IEnumerable<Models.Teacher>? teachersForTeacherId;


        protected int groupsForGroupIdCount;
        protected Models.Groups? groupsForGroupIdValue;
        protected int count;
        protected bool isEdit = false;
        protected bool addCliecked = false;
        protected async Task groupsForGroupIdLoadData(LoadDataArgs args)
        {
            try
            {
                var filter = $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')";
                //var result = await LangClientService.GetGroups(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                var result = await LangClientService.GetGroups();
                groupsForGroupId = result;
                groupsForGroupIdCount = result.Count();

            }
            catch (System.Exception ex)
            {
                Notify($"Error", $"Unable to load Group", NotificationSeverity.Error);
            }
        }

        protected int teachersForTeacherIdCount;
        protected Models.Teacher teachersForTeacherIdValue;
        protected async Task teachersForTeacherIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await LangClientService.GetTeachers(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: /*$"contains(UserId, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')",*/ null, orderby: $"{args.OrderBy}");
                teachersForTeacherId = result;
                teachersForTeacherIdCount = result.Count();

            }
            catch (System.Exception ex)
            {
                //NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Teacher" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                if (Session.End.TimeOfDay < Session.Start.TimeOfDay)
                {
                    Notify("Wrong Time Session", "Please enter a correct durre for session", NotificationSeverity.Warning);
                    return;
                }
                Session.End = Session.Start + (Session.End.TimeOfDay - Session.Start.TimeOfDay);
                isEdit = Session.Id != 0;
                dynamic result = isEdit ? await LangClientService.UpdateSession(id: Session.Id, Session) : await LangClientService.CreateSession(Session);
                dialogService.Close();

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            //dialogService.Close(null);
        }

        protected override async Task OnInitializedAsync()
        {
            if(SessionId != 0)
            {
                Session = await LangClientService!.GetSessionById(id: SessionId);
            }
            else
            {
                Session = new Session();
                Session.End = End;
                Session.Start = Start;
                if (IsForGroup)
                {
                    Session.GroupId = GroupOrTeacherId;
                }
                else
                {
                    Session.TeacherId = GroupOrTeacherId;
                }

            }
            
            
        }
    }
}