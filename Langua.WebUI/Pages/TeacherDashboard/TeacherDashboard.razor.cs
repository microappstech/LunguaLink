using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Radzen;

namespace Langua.WebUI.Pages.TeacherDashboard
{
    public partial class TeacherDashboardComponent:BasePage
    {
        [Inject] public IRepositoryCrudBase<GroupCandidates> CandidateService { get; set; }
        [Inject] private BaseService baseService { get; set; }
        public IEnumerable<GroupCandidates> Candidats { get; set; }
        public bool DataReady {  get; set; }
        protected override async Task OnInitializedAsync()
        {
            var resultToCandidates = CandidateService.GetAll();
            if(resultToCandidates.Succeeded)
            {
                //Candidats = resultToCandidates.Value;
                //baseService.Apply((IQueryable<Candidat>)Candidats, new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { { "include", new StringValues("Subject,Group") } }));
                Candidats = (IEnumerable<GroupCandidates>)baseService.Apply(resultToCandidates.Value, new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> { { "include", new StringValues("Subject,Group") } }));
                DataReady = true;
            }
        }
    }
}