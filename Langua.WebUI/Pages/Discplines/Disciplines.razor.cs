
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;

namespace Langua.WebUI.Pages.Discplines
{
    public partial class Disciplines:BasePage
    {
        [Inject] protected IRepositoryCrudBase<Subject> baseRepository { get; set; }
        public bool DataReady { get; set; } = false;
        protected IEnumerable<Subject> subjects { get; set; }
        protected override async Task OnInitializedAsync()
        {
            subjects = baseRepository.GetAll();
            
            
            
            DataReady = true;
        }

    }
}