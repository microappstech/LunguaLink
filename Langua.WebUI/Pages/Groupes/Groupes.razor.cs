using Radzen;
using Langua.Models;
using Langua.WebUI.Pages.Candidates;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using Langua.Repositories.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Langua.WebUI.Pages.Groupes
{
    public partial class GroupsComponent : BasePage
    {
        public RadzenDataGrid<Groups> GroupGrid;
        public RadzenDataGrid<Candidat> candidatGroup;
        [Inject] public IRepositoryCrudBase<Groups> repository { get; set; }
        public IEnumerable<Groups> Groups { get; set; }
        public IEnumerable<Candidat> Candidats { get; set; }
        public void OnExpand(TreeExpandEventArgs args)
        {

        }

        public async void Add()
        {
            var result = dialogService.OpenAsync<AddGroup>("Create New Group");
            await InvokeAsync(StateHasChanged);
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
         //   args.Expandable = args.Data.ShipCountry == "France" || args.Data.ShipCountry == "Brazil";
        }
    }
}