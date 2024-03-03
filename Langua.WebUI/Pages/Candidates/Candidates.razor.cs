
using System;
using System.Text;
using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Langua.WebUI.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
//using static Langua.WebUI.Pages.Components.LanguaGrid<dynamic>;


namespace Langua.WebUI.Pages.Candidates
{

    public partial class CandidatesComponent : BasePage
    {
        public IEnumerable<Candidat> candidates { get; set; }

        [Inject] private IRepositoryCrudBase<Candidat> baseRepository { get; set; }
        protected override Task OnInitializedAsync()
        {
            var candidatesResult = baseRepository.GetAll();
            candidates = candidatesResult.Value;
            return base.OnInitializedAsync();
        }
        public async Task Delete(Candidat candidat)
        {
            if (await Confirm(L["Confirmation"], L["Are you sure want to delete this candidate"]) == true)
            {

                var resultDelete = baseRepository.Delete(candidat);
                if (resultDelete.Succeeded)
                {
                    Notify("Success", "Suppression successfully finished",NotificationSeverity.Success);
                    dialogService.Close();
                }

            }
        }
        public async Task Edit(Candidat candidat)
        {
            var result = await dialogService.OpenAsync<EditCandidate>("Edit Componenet", new Dictionary<string, object> { { "Id", candidat.Id } });
            await InvokeAsync(StateHasChanged);
        }
        public async Task Add()
        {
           var result = await dialogService.OpenAsync<AddCandidate>("Add new candidate", null, new DialogOptions { Width = "50vw",  ShowClose = true });
            StateHasChanged();
        }
    }




}