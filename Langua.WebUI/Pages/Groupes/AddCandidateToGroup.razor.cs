using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Diagnostics.Contracts;

namespace Langua.WebUI.Pages.Groupes
{
    public partial class AddCandidateToGroupComponent: BasePage
    {
        [Parameter] public int groupId { get; set; }
        public Groups groupe { get; set; }
        public IEnumerable<Candidat> Candidates { get; set; }
        public List<Candidat> SelectedCandidates { get; set; }
        [Inject] IRepositoryCrudBase<Groups> GroupService { get; set; }
        [Inject] IRepositoryCrudBase<Candidat> CandidateService { get; set; }
        [Inject] IGroupCandidateService<GroupCandidates> GrCanService { get; set; }
        protected bool isloading;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isloading = true;
                var groupResult = await Task.Run(()=> GroupService.GetById(Convert.ToInt32(groupId)));
                if (groupResult.Succeeded)
                {
                    groupe = groupResult.Value;
                }
                var CandiResult = await Task.Run(()=>CandidateService.GetAll());
                if (CandiResult.Succeeded)
                {
                    Candidates = CandiResult.Value;
                }
            }
            finally
            {
                isloading = false;
                StateHasChanged();
            }
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                Rendered = true;
            }
        }
        bool Rendered = false;
        protected bool IsSaving;
        public async Task Submit()
        {
            try
            {
                IsSaving = true;
                var GResult = await Task.Run(()=> GrCanService.AddCandidateGroup(groupe, SelectedCandidates.Select(i => i.Id).ToList()));
                if (GResult.Succeeded)
                {
                    Notify("Success", "Group Successfuly created", Radzen.NotificationSeverity.Success);
                    dialogService.Close();
                }
            }
            finally
            {
                IsSaving = false;
            }
        }

    }
}