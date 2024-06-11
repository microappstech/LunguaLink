using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Langua.Shared.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Radzen;

namespace Langua.WebUI.Pages.TeacherDashboard
{
    public partial class TeacherDashboardComponent:BasePage
    {
        [Inject] public IRepositoryCrudBase<GroupCandidates>? GroupCandidateService { get; set; }

        [Inject] public IRepositoryCrudBase<Models.GroupTeacher>? GroupTeacherService { get; set; }
        
        public IEnumerable<Groups>? Groups { get; set; }
        public IEnumerable<Candidat>? Candidats { get; set; }

        public IEnumerable<Models.GroupTeacher>? groupTeachers { get; set; }
        public Teacher? Teacher { get; set; }
        public bool DataReady {  get; set; }
        public int NbRessource, NbGroups, NbCandidates;

        protected override async Task OnInitializedAsync()
        {
            await Security.InitializeAsync();
            await Security.IsAuthenticatedWidthRedirect();
            var r = await Security.IsInRole("ADMIN");
            var teacher = await baseService.GetEntiteByUserId<Teacher>(Security.User.Id, t => t.UserId == Security.User.Id);
            if(teacher == null)
            {
                Navigation.NavigateToLogin("/login");
            }

            var resultGroups = GroupTeacherService.GetByExpression($"TeacherId=={teacher.Id.ToString()}");
            if (resultGroups.Succeeded && resultGroups.Value.Count() > 0)
            {
                NbGroups = resultGroups.Value.Count();
                List<int> GroupTeacherIds = resultGroups.Value.Select(i => i.GroupId).ToList();
                await baseService.Apply(resultGroups.Value, new QueryCollection(new Dictionary<string, StringValues> { { "include", "Subject,Group" } }));
                groupTeachers = resultGroups.Value;

                Result<IQueryable<Candidat>>? resultCandidates = await LanguaService.GetCandidateForGroups(GroupTeacherIds);
            if (resultCandidates.Succeeded && resultCandidates.Value.Count() > 0 && GroupTeacherIds is not null)
            {
                var included = await baseService.Apply(resultCandidates.Value, new QueryCollection(new Dictionary<string, StringValues> { { "include", new StringValues("Group,Subject") } }));
                Candidats = included;
                Candidats = Candidats.Where(i=> GroupTeacherIds.Contains((int)i.GroupId));
                if(Candidats is not null) 
                    NbCandidates = Candidats.Where(i=>GroupTeacherIds.Contains((int)i.GroupId)).ToList().Count;
            }


            }
        }
    }
}