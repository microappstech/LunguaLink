using Langua.Models;
using Microsoft.AspNetCore.Components;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Langua.DataContext.Data;
using Radzen;
using System.Transactions;


namespace Langua.WebUI.Pages.Teachers
{
    public partial class EditTeacherComponent : BasePage
    {
        [Parameter] public int Id { get; set; }
        public Teacher teacher { get; set; }
        public bool ChangePass { get; set; }
        [Inject] private IRepositoryCrudBase<Teacher> _repository { get; set; }
        protected override Task OnInitializedAsync()
        {
            var TResult = _repository.GetById(Convert.ToInt32(Id));
            if (TResult.Succeeded)
            {
                teacher = TResult.Value;
            }
            return base.OnInitializedAsync();
        }

        protected async Task HandleValidSubmit()
        {
            var result = _repository.Update(teacher);
            if (result.Succeeded)
            {
                notificationService.Notify(NotificationSeverity.Success, "Edition Successful Completed");
                StateHasChanged();
                dialogService.Close(null);
            }
            await Task.CompletedTask;
        }
        public async Task loadImage(InputFileChangeEventArgs args)
        {
            MemoryStream ms = new MemoryStream();
            await args.File.OpenReadStream().CopyToAsync(ms);
            byte[] bytes = ms.ToArray();
            string base64 = Convert.ToBase64String(bytes);
            teacher.Photo = "data:image/png;base64," + base64;
            await Task.CompletedTask;
        }

    }
}