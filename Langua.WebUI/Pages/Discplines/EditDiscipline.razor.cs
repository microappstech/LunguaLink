using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Langua.WebUI.Pages.Discplines
{
    public partial class EditDisciplineComponent:BasePage
    {
        [Parameter] public int Id { get; set; } 
        public Subject subject {  get; set; }
        public bool DataReady { get; set; }
        public string srcImage { get; set; }    
        [Inject] public IRepositoryCrudBase<Subject> _repository { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var SubResult = _repository.GetById(Convert.ToInt32(Id));
            if(SubResult.Succeeded)
            {
                DataReady = true;
                subject = SubResult.Value;
                srcImage = subject.Photo;
            }
            await Task.CompletedTask;
        }
        public async Task HandleValidSubmit()
        {
            var result = _repository.Update(subject);
            if (result.Succeeded)
            {
                Notify("Successful Edition", "The subject Successfully Edited", NotificationSeverity.Success);
                dialogService.Close(subject);
            }
            else
            {
                Notify("Failed", "Edition Failed", NotificationSeverity.Error);
            }
            await Task.CompletedTask;
        }
        public async void loadImage(InputFileChangeEventArgs args)
        {
            MemoryStream memoryStream = new MemoryStream();
            await args.File.OpenReadStream().CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64 = Convert.ToBase64String(bytes);

            subject.Photo = "data:image/png;base64," + base64;
            srcImage = "data:image/png;base64," + base64;
        }
    }
}