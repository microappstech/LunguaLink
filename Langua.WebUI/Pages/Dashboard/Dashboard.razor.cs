using Langua.Models;
using Langua.Repositories.Services;
using Langua.Repositories.Services.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Langua.WebUI.Pages.Dashboard
{
    public partial class DashboardComponent:BasePage
    {
        public IEnumerable<Candidat> Candidates { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public int NbTeacher;
        public int NbCandidat;
        public int NbGroups;

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
            
            await Security.IsAuthenticatedWidthRedirect();
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

            NbGroups = await baseService.NBItems<Groups>(); 
        }
        public async Task SendMailToCandidate()
        {
            mailService.SendMail("Test Subject", "body Mail ", "Hamzamouddakur@gmail.com", "Hamza");
            Validation validate = new Validation();
            List<string> mails = new List<string>
            {
                "oneblack342@gmail.com",
                "oneblsdQSDack342@gmail.com",
                "hamzamouddakur@gmail.com",
                "sdfgsdgfgqsdjhjhqsdgjh@gmail.com",
                
            };
            foreach (var item in mails)
            {
                validate.ValidateMail(item);
            }
        }
    }
}