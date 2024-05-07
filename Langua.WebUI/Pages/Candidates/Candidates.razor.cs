
using System;
using System.Text;
using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Langua.WebUI.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Primitives;
using Radzen;
using Radzen.Blazor;
//using static Langua.WebUI.Pages.Components.LanguaGrid<dynamic>;


namespace Langua.WebUI.Pages.Candidates
{

    public partial class CandidatesComponent : BasePage
    {
        public IEnumerable<Candidat> candidates { get; set; }
        public List<Candidat> fcandidates { get; set; }
        public RadzenDataGrid<Candidat>? grid0;
        [Inject] private IRepositoryCrudBase<Candidat> baseRepository { get; set; }
        public void filter(string s, string on)
        {
            fcandidates = candidates.Where(i=>i.FullName == s).ToList();
            if (string.IsNullOrEmpty(s))
                fcandidates = candidates.ToList();
        }
        protected override Task OnInitializedAsync()
        {

            var candidatesResult = baseRepository.GetAll();
            
            IQueryCollection queryCollection = 
                new QueryCollection(
                    new Dictionary<string, StringValues> { { "include", new StringValues("Subject") } }
                    );

            baseService.Apply(candidatesResult.Value, queryCollection);
            candidates = (IEnumerable<Candidat>)baseService.Apply(candidatesResult.Value, queryCollection);
            fcandidates = candidates.ToList();
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
                    await grid0.Reload();
                }
                else
                {
                    Notify("Error", "Something Happened wrong", NotificationSeverity.Error);
                }

            }
        }
        public async Task Edit(Candidat candidat)
        {
            var result = await dialogService.OpenAsync<Langua.WebUI.Pages.Candidates.EditCandidate>("Edit Componenet", new Dictionary<string, object> { { "Id", candidat.Id } });
            await grid0.Reload();
        }
        public async Task Add()
        {
           var result = await dialogService.OpenAsync<Langua.WebUI.Pages.Candidates.AddCandidate>("Add new candidate", null, new DialogOptions { Width = "50vw",  ShowClose = true });
            await grid0.Reload();
        }
    }




}