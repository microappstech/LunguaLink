
using System;
using System.Text;
using Langua.DataContext.Data;
using Langua.Models;
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


        protected override Task OnInitializedAsync()
        {
            candidates = baseRepository.GetAll();
            return base.OnInitializedAsync();
        }
        public async Task Delete(Candidat candidat)
        {
            if (await Confirm(L["Confirmation"], L["Are you sure want to delete this candidate"]) == true)
            {



                Notify("Success", "Suppression successfully finished");
            }
        }
        public async Task Edit(Candidat candidat)
        {
            //var dialogResult = await DialogService.OpenAsync<EditContenu>("Editer la ressource", new Dictionary<string, object>() { { "id", args.Data.id } }, new DialogOptions() { Width = "850px", ShowClose = true });
        }
        public async Task Add()
        {
           var result = await dialogService.OpenAsync<AddCandidate>("Add new candidate", null, new DialogOptions { Width = "50vw",  ShowClose = true });
            StateHasChanged();
        }
    }




}