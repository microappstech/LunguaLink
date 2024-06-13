using Langua.Models;
using Langua.Repositories.Services;
using Langua.Repositories.Services.Validation;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;

namespace Langua.WebUI.Pages.Dashboard
{
    public partial class DashboardComponent:BasePage
    {
        private HubConnection _chatGroupHub;
        public IEnumerable<Candidat>? Candidates { get; set; }
        public IEnumerable<Teacher> ?Teachers { get; set; }
        public int NbTeacher;
        public int NbCandidat;
        public int NbGroups, NbManagers, NbDepartements, NbGrTeachers, NbGrCandidates;

        [Inject] public BaseService? baseService { get; set; }



        private class IssueGroup
        {
            public int Count { get; set; }
            public DateTime Week { get; set; }
        }
        protected override async Task OnInitializedAsync()
        {
            await Security!.InitializeAsync();
            await Security!.IsAuthenticatedWidthRedirect();
            NbTeacher = await baseService!.NBItems<Teacher>();
            NbCandidat = await baseService.NBItems<Candidat>();
            NbGroups = await baseService.NBItems<Groups>();
            NbDepartements = await baseService.NBItems<Department>();
            NbManagers = await baseService.NBItems<Models.Manager>();
            NbGrCandidates = await baseService.NBItems<GroupCandidates>();
            NbGrTeachers = await baseService.NBItems<Models.GroupTeacher>();
            




            _chatGroupHub = new HubConnectionBuilder()
                .WithUrl(Navigation!.ToAbsoluteUri(ApiControllers.LanguaHub.ChatHub.ChatGroupEndPoint))
                .WithAutomaticReconnect()
                .Build();
            _chatGroupHub.On<string, string, string>("SendMessageToGroup", async (GroupId, FromUserId, Message) =>
            {
                Notify("New Message", Message, Radzen.NotificationSeverity.Info, 50000);
            });
            try
            {
                await _chatGroupHub.StartAsync();
            }catch(Exception ex)
            {
                Notify("Error start connection", ex.Message, Radzen.NotificationSeverity.Error, 50000);
            }
        }
    }
}