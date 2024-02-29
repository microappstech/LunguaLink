using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Account.Pages.Manage;
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
            string Pass = candidate.Email.Substring(0, candidate.Email.IndexOf("@"));
            Pass = Pass.Length < 4 ? $"User_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Second}_{DateTime.Now.Microsecond}!" : "";
            ApplicationUser _user = new ApplicationUser()
            {
                Email = candidate.Email,
                UserName = candidate.Email,
                Password = candidate.Password,
                NormalizedUserName = candidate.FullName,
                PhoneNumber = candidate.FullName
            };
            var user = await Security.RegisterUser(_user);
            if (user is not null)
            {
                candidate.UserId = user.Id;
                var result = _repository.Add(candidate);
            }
        }
        public void Close()
        {
            dialogService.Close(null);
        }
    }
}