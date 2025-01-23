using Langua.Models;
using Langua.Repositories.Interfaces;
using Langua.WebUI.Pages.Candidates;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Langua.WebUI.Pages.Discplines
{
    public partial class AddDiscplineComponent : BasePage
    {
        protected Subject subject {  get; set; }
        [Inject] private IRepositoryCrudBase<Subject> _repository {  get; set; }

        public string srcImage { get; set; }
        public async Task HandleValidSubmit()
        {
            var result = _repository.Add(subject);
            if (result.Succeeded)
            {
                notificationService.Notify(NotificationSeverity.Success, "Creation Successful Completed");
                StateHasChanged();
                dialogService.Close();
            }
            await Task.CompletedTask;
        }
        protected override Task OnInitializedAsync()
        {
            subject = new Subject();
            return base.OnInitializedAsync();
        }
        public async Task loadImage(InputFileChangeEventArgs args)
        {
            MemoryStream memoryStream = new MemoryStream();
            await args.File.OpenReadStream().CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);

            subject.Photo = "data:image/png;base64," + base64;
            srcImage = "data:image/png;base64," + base64;
            await Task.CompletedTask;
        }

    }
}