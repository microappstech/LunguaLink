using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Diagnostics.Contracts;

namespace Langua.WebUI.Pages.Groupes
{
    public partial class AddCandidateToGroupComponent:BasePage
    {
        [Parameter] public int groupId { get; set; }
        public Groups groupe { get; set; }
        public IEnumerable<Candidat> Candidates { get; set; }
        public List<int> SelectedCandidates { get; set; }
        [Inject] IRepositoryCrudBase<Groups> GroupService { get; set; }
        [Inject] IRepositoryCrudBase<Candidat> CandidateService { get; set; }
        [Inject] IGroupCandidateService<GroupCandidates> GrCanService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var groupResult = GroupService.GetById(Convert.ToInt32(groupId));
            if (groupResult.Succeeded)
            {
                groupe = groupResult.Value;
            }
            var CandiResult = CandidateService.GetAll();
            if(CandiResult.Succeeded)
            {
                Candidates = CandiResult.Value;
            }
        }

        public async Task Submit()
        {
            var GResult = GrCanService.AddCandidateGroup(groupe,SelectedCandidates);
            if (GResult.Succeeded)
            {
                Notify("Success", "Group Successfuly created", Radzen.NotificationSeverity.Success);
                dialogService.Close();
            }
        }

    }
}