using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Transactions;

namespace Langua.WebUI.Pages.Candidates
{
    public partial class EditCandidateComponent:BasePage
    {
        protected string _oldmail;
        protected string fileName;
        protected long? fileSize;
        public bool Submited { get; set; }
        [Parameter] public int Id { get; set; }
        [Inject] private IRepositoryCrudBase<Candidat> _repository { get; set; }
        public bool Changepass { get; set; }

        public bool ChangePassword { get; set; }
        protected Candidat candidate { get; set; }
        public bool DataReady { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var CResult = _repository.GetById(Convert.ToInt32(Id));
            if (CResult.Succeeded)
            {
                candidate = CResult.Value;
                _oldmail = candidate.Email;
                await Task.CompletedTask;
            }

        }

        public void OnError(UploadErrorEventArgs args, string name)
        {
            Notify(L["Error"], args.Message, NotificationSeverity.Error);
        }
        protected async Task HandleValidSubmit()
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TimeSpan.FromMinutes(3), TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = _repository.Update(candidate);
                if (result.Succeeded)
                {
                    notificationService.Notify(NotificationSeverity.Success, "Edition Successful Completed");
                    StateHasChanged();
                    dialogService.Close(null);
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