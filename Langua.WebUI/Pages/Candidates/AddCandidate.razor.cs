using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Account.Pages.Manage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.Transactions;

namespace Langua.WebUI.Pages.Candidates
{
    public partial class AddCandidateComponent : BasePage
    {
        [Inject] private IRepositoryCrudBase<Candidat> _repository { get; set; }
        [Inject] private IRepositoryCrudBase<Subject> _repositorySubjects { get; set; }

        protected Candidat candidate { get; set; }
        public IEnumerable<Subject> subjects { get; set; }
        public bool DataReady { get; set; }
        protected override async Task OnInitializedAsync()
        {
            candidate = new Candidat();
            var SubjectResult = _repositorySubjects.GetAll();
            if (SubjectResult.Succeeded)
            {
                subjects = SubjectResult.Value;
            }
            
        }
        protected async Task HandleValidSubmit()
        {
            ApplicationUser _user = new ApplicationUser()
            {
                Email = candidate.Email,
                UserName = candidate.Email,
                Password = candidate.Password,
                NormalizedUserName = candidate.FullName,
                PhoneNumber = candidate.FullName
            };
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                var user = await Security.RegisterUser(_user);
                if (user is not null)
                {
                    candidate.UserId = user.Id;
                    var result = _repository.Add(candidate);
                    if (result.Succeeded)
                    {
                        notificationService.Notify(NotificationSeverity.Success, "Creation Successful Completed");
                        StateHasChanged();
                        dialogService.Close(null);
                    }
                }
                scope.Complete();
                
            }
        }
        public void Close()
        {
            dialogService.Close(null);
        }
        public async void loadImage(InputFileChangeEventArgs args)
        {
            MemoryStream memoryStream = new MemoryStream();
            await args.File.OpenReadStream().CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);
            
            candidate.Photo = "data:image/png;base64," + base64;

        }
    }
}