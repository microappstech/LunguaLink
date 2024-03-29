using Radzen;
using Langua.Models;
using Langua.WebUI.Pages.Candidates;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using Langua.Repositories.Interfaces;
using Microsoft.Extensions.Primitives;
using System.Security.Principal;

namespace Langua.WebUI.Pages.Groupes
{
    public partial class GroupsComponent : BasePage
    {
        
        public RadzenDataGrid<Groups> grid;
        public RadzenDataGrid<Candidat> gridCandidate;
        [Inject] public IRepositoryCrudBase<Groups> repository { get; set; }
        [Inject] IGroupCandidateService<GroupCandidates> GrCanService { get; set; }
        public IEnumerable<Groups> Groups { get; set; }
        public IEnumerable<Candidat> Candidats { get; set; }
        public void OnExpand(TreeExpandEventArgs args)
        {

        }

        public async void Add()
        {
            var result = dialogService.OpenAsync<AddGroup>(L["Create New Group"]);
            await grid.Reload();
        }
        public async Task Delete(Groups group)
        {
            if (await Confirm(L["Confirmation"], L["Are you sure want to delete this group"]) == true)
            {
                var resultDelete = repository.Delete(group);
                if (resultDelete.Succeeded)
                {
                    Notify(L["Success"], L["Suppression successfully finished"], NotificationSeverity.Success);
                    dialogService.Close();
                }
                else
                {
                    Notify(L["Error"], L["Somtheing worng"],NotificationSeverity.Error);
                }
                await grid.Reload();
            }
        }
        public async Task Edit(Groups group)
        {
            var result = await dialogService.OpenAsync<EditGroup>(@L["Edit the group "], new Dictionary<string, object> { { "Id", group.Id } });
            await grid.Reload();
        }

        public async Task AddCandidatToGroup(Groups group)
        {
            var reult = await dialogService.OpenAsync<AddCandidateToGroup>($"Add new candidate to {group.Name} ", new Dictionary<string, object> { { "groupId", group.Id } });
            await gridCandidate.Reload();
        }

        protected override Task OnInitializedAsync()
        {
            var GResult = repository.GetAll();
            if (GResult.Succeeded)
            {
                Groups = (IEnumerable<Groups>)baseService.Apply(GResult.Value, 
                    new QueryCollection(new Dictionary<string, StringValues> { { "include", "GroupeMessages,Candidats" } })
                    );
            }
            return base.OnInitializedAsync();
        }
        public void RowRender(RowRenderEventArgs<Groups> args)
        {

        }
        public void OnExpend(Langua.Models.Groups group)
        {
            var result= GrCanService.GetCandidateByGroupId(group.Id);
            if (result.Succeeded)
            {
                var Candidates = (IEnumerable<Candidat>)baseService.Apply((IQueryable<Candidat>)result.Value, new QueryCollection(new Dictionary<string, StringValues> { { "include", "Subject" } }));
                group.Candidats = Candidates.ToList();
            }
        }
    }
}