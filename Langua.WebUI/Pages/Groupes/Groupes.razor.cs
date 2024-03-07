using Radzen;
using Langua.Models;
using Langua.WebUI.Pages.Candidates;
using Radzen.Blazor;

namespace Langua.WebUI.Pages.Groupes
{
    public partial class GroupsComponent : BasePage
    {
        public RadzenDataGrid<Groups> GroupGrid;
        public IEnumerable<Groups> Groups { get; set; }
        public void OnExpand(TreeExpandEventArgs args)
        {

        }

        public void Add()
        {
            //var result = await dialogService.OpenAsync<AddCandidate>("Add new candidate", null, new DialogOptions { Width = "50vw", ShowClose = true });

        }
        public async Task Delete(Groups group)
        {
            if (await Confirm(L["Confirmation"], L["Are you sure want to delete this group"]) == true)
            {
                //var resultDelete = baseRepository.Delete(candidat);
                //if (resultDelete.Succeeded)
                //{
                //    Notify("Success", "Suppression successfully finished", NotificationSeverity.Success);
                //    dialogService.Close();
                //}
            }
        }
        public async Task Edit(Groups group)
        {
            //var result = await dialogService.OpenAsync<EditCandidate>("Edit Componenet", new Dictionary<string, object> { { "Id", candidat.Id } });
            await InvokeAsync(StateHasChanged);
        }



        protected override Task OnInitializedAsync()
        {
            Groups = new List<Groups>()
            {
                new Groups { Name = "French Group", Description ="Frensh Group" },
                new Groups { Name = "Anglais Group", Description ="Anglais Group" }
            };
            return base.OnInitializedAsync();
        }
    }
}