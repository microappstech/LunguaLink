using Langua.Models;
using Langua.Repositories.Services;
using Langua.Repositories.Services.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Langua.WebUI.Pages.Dashboard
{
    public partial class DashboardComponent:BasePage
    {
        public IEnumerable<Candidat>? Candidates { get; set; }
        public IEnumerable<Teacher> ?Teachers { get; set; }
        public int NbTeacher;
        public int NbCandidat;
        public int NbGroups, NbManagers, NbDepartements;

        [Inject] public BaseService? baseService { get; set; }



        private class IssueGroup
        {
            public int Count { get; set; }
            public DateTime Week { get; set; }
        }
        protected override async Task OnInitializedAsync()
        {
            
            await Security!.IsAuthenticatedWidthRedirect();
            NbTeacher = await baseService!.NBItems<Teacher>();
            NbCandidat = await baseService.NBItems<Candidat>();
            NbGroups = await baseService.NBItems<Groups>(); 
            NbDepartements = await baseService.NBItems<Department>();
            NbManagers = await baseService.NBItems<Models.Manager>();

        }
    }
}