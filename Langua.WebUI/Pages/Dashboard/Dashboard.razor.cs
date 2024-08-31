using Langua.Models;
using Langua.Repositories.Services;
using Langua.Repositories.Services.Validation;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Langua.WebUI.Pages.Dashboard
{
    public partial class DashboardComponent:BasePage
    {
        private HubConnection? _chatGroupHub;
        public IEnumerable<Candidat>? Candidates { get; set; }
        public CandidateAlongGroup[]? GroupsStat;
        public SessionByGroup[]? NbSessionByGroups;
        public TotalDureeSessions[]? TotalDureeSessions;
        public IEnumerable<Teacher> ?Teachers { get; set; }
        public int NbGroups, NbManagers, NbDepartements, NbGrTeachers, NbGrCandidates, NbCandidat, NbTeacher, NbUsers;




        private class IssueGroup
        {
            public int Count { get; set; }
            public DateTime Week { get; set; }
        }
        protected override async Task OnInitializedAsync()
        {
            await Security!.InitializeAsync();
            await Security!.IsAuthenticatedWidthRedirect();
            if (await Security.IsInRole("ADMIN"))
            {
                NbTeacher = await baseService!.NBItems<Teacher>();
                NbCandidat = await baseService.NBItems<Candidat>();
                NbGroups = await baseService.NBItems<Groups>();
                NbDepartements = await baseService.NBItems<Department>();
                NbManagers = await baseService.NBItems<Models.Manager>();
                NbGrCandidates = await baseService.NBItems<GroupCandidates>();
                NbGrTeachers = await baseService.NBItems<Models.GroupTeacher>();
                NbUsers = await baseService.NBItems<ApplicationUser>();
                var resGrCan = await LanguaService.candidateAlongGroups();
                var ResSByGr = await LanguaService.NbSessionByGroup();
                var TotDureSes = await LanguaService.TotalDureeSessions();
                if (resGrCan.Succeeded)
                    GroupsStat = resGrCan.Value.ToArray();
                if(ResSByGr.Succeeded)
                    NbSessionByGroups = ResSByGr.Value.ToArray();
                if (TotDureSes.Succeeded)
                    TotalDureeSessions = TotDureSes.Value.ToArray();

            }
            else if(await Security.IsInRole("MANAGER"))
            {
                var Manager =await LanguaService.GetManagerByUserId(Security.User.Id);
                if(!Manager.Succeeded)
                {
                    Navigation.NavigateToLogin("login");
                }
                         
                NbTeacher = await baseService!.NBItemsForManager<Teacher>(Manager.Value.Id);
                NbCandidat = await baseService.NBItemsForManager<Candidat>(Manager.Value.Id);
                NbUsers = await baseService.NBItemsForManager<ApplicationUser>(Manager.Value.Id);
                NbDepartements = await baseService.NBItemsForManager<Department>(Manager.Value.Id);
                NbGroups = await baseService.NBItemsForManager<Groups>(Manager.Value.Id);
                NbGrTeachers = await baseService.NBItemsForManager<Models.GroupTeacher>(Manager.Value.Id);
                NbGrCandidates = await baseService.NBItemsForManager<Models.GroupCandidates>(Manager.Value.Id);
                var resGrCan = await LanguaService.candidateAlongGroups(Manager.Value.DepartmentId);
                var ResSByGr = await LanguaService.NbSessionByGroup(Manager.Value.DepartmentId);
                var TotDureSes = await LanguaService.TotalDureeSessions(Manager.Value.DepartmentId);
                if (resGrCan.Succeeded)
                    GroupsStat = resGrCan.Value.ToArray();
                if (ResSByGr.Succeeded)
                    NbSessionByGroups = ResSByGr.Value.ToArray();
                if (TotDureSes.Succeeded)
                    TotalDureeSessions = TotDureSes.Value.ToArray();
            }





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