using Langua.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Langua.Repositories.Interfaces;
using Langua.DataContext.Data;
using Radzen;
using System.Transactions;
using System.Data.Common;

namespace Langua.WebUI.Pages.Teachers
{
    public partial class AddTeacherComonent:BasePage
    {
        [Inject] private IRepositoryCrudBase<Teacher> _repository { get; set; }
        [Inject] private IRepositoryCrudBase<Subject> _repositorySubjects { get; set; }

        protected Teacher teacher { get; set; }
        public IEnumerable<Subject> subjects { get; set; }
        public bool DataReady { get; set; }
        protected override async Task OnInitializedAsync()
        {
            teacher = new Teacher();
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
                Email = teacher.Email,
                UserName = teacher.Email,
                Password = teacher.Password,
                NormalizedUserName = teacher.FullName,
                PhoneNumber = teacher.Phone
            };
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                var user = await Security.RegisterUser(_user);
                if (user is not null)
                {
                    teacher.UserId = user.Id;
                    var result = _repository.Add(teacher);
                    if (result.Succeeded)
                    {
                        Notify("Success", "Creation Successful Completed", NotificationSeverity.Success);
                        scope.Complete();
                        dialogService.Close(null);
                    }
                    else
                    {
                        scope.Dispose();
                        Notify("Failed", "Something Wrong", NotificationSeverity.Error);
                    }
                }
                else
                {
                    scope.Dispose();
                }

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

            teacher.Photo = "data:image/png;base64," + base64;

        }
    }
}