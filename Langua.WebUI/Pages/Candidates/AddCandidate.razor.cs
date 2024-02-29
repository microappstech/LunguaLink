using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Langua.WebUI.Pages.Candidates
{
    public partial class AddCandidateComponent : BasePage
    {
        [Inject] private IRepositoryCrudBase<Candidat> _repository { get; set; }
        protected Candidat candidate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            candidate = new Candidat();
            
        }
        protected async Task HandleValidSubmit()
        {
            var result = _repository.Add(candidate);
        }
        public void Close()
        {
            dialogService.Close(null);
        }
    }
}