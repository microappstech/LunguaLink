using Langua.Models;
using Langua.Repositories.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Langua.Web.Components.Pages.Dashboard
{
    public partial class DashboardComponent:BasePage
    {
        public IEnumerable<Candidat> Candidates { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public int NbTeacher;
        public int NbCandidat;

        [Inject] public BaseService baseService { get; set; }


        IEnumerable<IssueGroup> openIssuesByDate;
        IEnumerable<IssueGroup> closedIssuesByDate;

        private class IssueGroup
        {
            public int Count { get; set; }
            public DateTime Week { get; set; }
        }
        protected override async Task OnInitializedAsync()
        {
            
            await Security.IsAuthenticated();
            openIssuesByDate = new List<IssueGroup>()
            {
                new IssueGroup { Count=12,Week=new DateTime(2020,12,01) },
                new IssueGroup { Count=12,Week=new DateTime(2020,12,02) },
                new IssueGroup { Count=18,Week=new DateTime(2020,12,04) },
                new IssueGroup { Count=12,Week=new DateTime(2020,12,06) },
                new IssueGroup { Count=10,Week=new DateTime(2020,12,08) },
                new IssueGroup { Count=12,Week=new DateTime(2020,12,10) },
                new IssueGroup { Count=10,Week=new DateTime(2020,12,15) },
                new IssueGroup { Count=13,Week=new DateTime(2020,12,20) },
                new IssueGroup { Count=19,Week=new DateTime(2020,12,25) },
                new IssueGroup { Count=10, Week = new DateTime(2022,12,30) }
            };
            closedIssuesByDate = new List<IssueGroup>()
            {
                new IssueGroup{Count=12,Week=DateTime.UtcNow},
                new IssueGroup{Count=10,Week=new DateTime(1999,12,15)},
                new IssueGroup{Count=13,Week=new DateTime(2020,01,15)},
                new IssueGroup{Count=18,Week=new DateTime(2020,01,15)},
            };
            NbTeacher = await baseService.NBItems<Teacher>();
            NbCandidat = await baseService.NBItems<Candidat>();

        }
    }
}